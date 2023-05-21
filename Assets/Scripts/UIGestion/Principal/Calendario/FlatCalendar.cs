/**
 * Flat Calendar
 * 
 * This class manage logic of Flat Calendar
 *
 * @version 1.0
 * @author  Gerardo Ritacco
 * @email   gerardo.ritacco@3dresearch.it
 * @company 3dresearchsrl
 * @website http://www.3dresearch.it/
 * 
 * Copyright © 2016 by 3dresearchsrl
 *
 * All rights reserved. No part of this publication may be reproduced, distributed, 
 * or transmitted in any form or by any means, including photocopying, recording, or 
 * other electronic or mechanical methods, without the prior written permission of the 
 * publisher, except in the case of brief quotations embodied in critical reviews and 
 * certain other noncommercial uses permitted by copyright law. For permission requests, 
 * write to the publisher, addressed “Attention: Permissions Coordinator,” at the address below.
 */

#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using UnityEngine;
using System.Collections;
using System.Globalization;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;


public class FlatCalendar : MonoBehaviour
{

	public Text descripcionEvento;

	/**
	 * Max day slots (DO NOT CHANGE THIS VALUE)
	 */
	public static readonly int max_day_slots = 37;


	/**
	 * List of Sprites
	 */
	public Sprite[] sprites;

	/**
	 * Current UI Style
	 */
	public int current_UiStyle;

	/**
	 * Event Structure Object
	 */ 
	public struct EventObj
	{
		public string name;
		public string description;

		public EventObj(string _name, string _description)
		{
			name 		= _name;
			description = _description;
		}

		public void print()
		{
			Debug.Log("Nombre del evento: " + name + " Descripción: " + description);
		}
	}

	/**
	 * Time Structure Object 
	 */
	public struct TimeObj
	{
		public int    year;
		public int    month;
		public int    day;
		public int    totalDays;
		public string dayOfWeek;
		public int    dayOffset;

		public TimeObj(int _year,int _month,int _day, int _totalDays, string _dayOfWeek, int _dayOffset)
		{
			year      = _year;
			month     = _month;
			day       = _day;
			totalDays = _totalDays;
			dayOffset = _dayOffset;
			dayOfWeek = _dayOfWeek;
		}

		public void print()
		{
			Debug.Log("Año:"+year+" Mes:"+month+" Día:"+day+" Día de la semana:"+dayOfWeek);
		}
	}


	/**
	 * Gameobjects Fields 
	 */
	GameObject btn_nextMonth;
	GameObject btn_prevMonth;
	GameObject btn_calendar;
	GameObject label_year;
	GameObject label_month;
	GameObject label_dayOfWeek;
	GameObject label_dayNumber;
	GameObject label_numberEvents;


	/**
	 * Current Time 
	 */
	public TimeObj currentTime;


	/*+
	 * Event List 
	 */
	public static Dictionary<int,Dictionary<int,Dictionary<int,List<EventObj>>>> events_list = new Dictionary<int, Dictionary<int, Dictionary<int, List<EventObj>>>>(); // <Year,<Month,<Day,Number of Events>>>

	/**
	 * Delegate Callbacks 
	 */
	public delegate void Delegate_OnDaySelected(TimeObj time);
	public delegate void Delegate_OnEventSelected(TimeObj time, List<EventObj> evs);
	public delegate void Delegate_OnMonthChanged(TimeObj time);
	public delegate void Delegate_OnNowDay(TimeObj time);
	public Delegate_OnDaySelected   delegate_ondayselected;
	public Delegate_OnEventSelected delegate_oneventselected;
	public Delegate_OnMonthChanged  delegate_onmonthchanged;
	public Delegate_OnNowDay		 delegate_onnowday;


	// Use this for initialization
	public void initFlatCalendar()
	{
		// Getting ui references
		btn_nextMonth      = GameObject.Find("Right_btn");
		btn_prevMonth      = GameObject.Find("Left_btn");
		btn_calendar       = GameObject.Find("Calendar_Btn");
		label_year         = GameObject.Find("Year");
		label_month        = GameObject.Find("Month");
		label_dayOfWeek    = GameObject.Find("Day_Title1");
		label_dayNumber    = GameObject.Find("Day_Title2");
		label_numberEvents = GameObject.Find("NumberEvents");

		// Add Event Listeners
		addEventsListener();
		
		// Apply UI Color style

		FlatCalendarStyle.changeUIStyle(current_UiStyle);
		
		
		// Set current time
		setCurrentTime(currentTime);

		// Initialize event list
		events_list = new Dictionary<int, Dictionary<int, Dictionary<int,List<EventObj>>>>();
		
		// Update Calendar with Current Data
		updateCalendar(currentTime.month,currentTime.year);
		
		// Mark Current Day
		markSelectionDay(currentTime.day);
		
		// Update Label Event
		updateUiLabelEvents(currentTime.year,currentTime.month,currentTime.day);

		Debug.Log("FECHA: "+ TimeManagerScript.Instance.CogerFecha());

		// Set current time
		SetCurrentTime(TimeManagerScript.Instance.CogerFecha());
	}

	void Start()
	{
		descripcionEvento = GameObject.Find("TxtNoticia").GetComponent<Text>();
		
		installDemoData();
		
	}

	void Update () { }

	//
	void OnEnable()
	{
		TimeManagerScript.Instance.cambioFecha.AddListener(SetCurrentTime);
	}
	
	void OnDisable()
	{
		TimeManagerScript.Instance.cambioFecha.RemoveListener(SetCurrentTime);
	}

	public void SetCurrentTime(DateTime fechaActualizada)
	{
		currentTime.year      = fechaActualizada.Year;
		currentTime.month     = fechaActualizada.Month;
		currentTime.day       = fechaActualizada.Day;
		currentTime.dayOfWeek = fechaActualizada.DayOfWeek.ToString();
		currentTime.totalDays = System.DateTime.DaysInMonth(currentTime.year,currentTime.month);
		currentTime.dayOffset = getIndexOfFirstSlotInMonth(currentTime.year,currentTime.month);
		
		// Update Calendar with Current Data
		updateCalendar(currentTime.month,currentTime.year);
		
		// Mark Current Day
		markSelectionDay(currentTime.day);
		
		// Update Label Event
		updateUiLabelEvents(currentTime.year,currentTime.month,currentTime.day);

	}

	// Update Calendar
	public void updateCalendar(int month_number, int year)
	{
		// Populate day slots
		populateAllSlot(month_number,year);

		// Update Year and Month Label
		label_year.GetComponent<Text>().text      = "" + currentTime.year;
		label_month.GetComponent<Text>().text     = getMonthStringFromNumber(currentTime.month);
	}

	public void refreshCalendar()
	{
		populateAllSlot(currentTime.month,currentTime.year);
	}

	/* Get Month String from Montth Number 
	 * 
	 * Example: Genuary <====> 1
	 */
	string getMonthStringFromNumber(int month_number)
	{
		string month = "";

		if(month_number == 1) month = "Enero";
		if(month_number == 2) month = "Febrero";
		if(month_number == 3) month = "Marzo";
		if(month_number == 4) month = "Abril";
		if(month_number == 5) month = "Mayo";
		if(month_number == 6) month = "Junio";
		if(month_number == 7) month = "Julio";
		if(month_number == 8) month = "Agosto";
		if(month_number == 9) month = "Septiembre";
		if(month_number == 10) month = "Octubre";
		if(month_number == 11) month = "Noviembre";
		if(month_number == 12) month = "Diciembre";

		return month;
	}

	/* 
	 * Get Day of Week From Year, Month and Day
	 * 
	 * Example: Monday <===> 2016,1,2
	 */
	string getDayOfWeek(int year, int month, int day)
	{
		System.DateTime dateValue = new System.DateTime(year,month,day);

		return dateValue.DayOfWeek.ToString();
	}

	/* 
	 * Get index of first slot where start day numeration
	 */
	int getIndexOfFirstSlotInMonth(int year, int month)
	{
		int indexOfFirstSlot = 0;

		System.DateTime dateValue = new System.DateTime(year,month,1);
		string dayOfWeek          = dateValue.DayOfWeek.ToString();

		//No podemos cambiar los dias a español porque si no pierden el Slot y hay error y los meses siempre empiezan en lunes
		if(dayOfWeek == "Monday")    indexOfFirstSlot = 0;
		if(dayOfWeek == "Tuesday")   indexOfFirstSlot = 1;
		if(dayOfWeek == "Wednesday") indexOfFirstSlot = 2;
		if(dayOfWeek == "Thursday")  indexOfFirstSlot = 3;
		if(dayOfWeek == "Friday")    indexOfFirstSlot = 4;
		if(dayOfWeek == "Saturday")  indexOfFirstSlot = 5;
		if(dayOfWeek == "Sunday")    indexOfFirstSlot = 6;

		return indexOfFirstSlot;
	}

	/*
	 * Disable all day slot 
	 */
	void disableAllSlot()
	{
		for(int i = 0; i < max_day_slots; i++)
			disableSlot(i+1);
	}

	/* 
	 * Disable day slot
	 */
	void disableSlot(int numSlot)
	{
		GameObject day_slot = GameObject.Find("Slot_"+ (numSlot));
		day_slot.GetComponent<Button>().enabled = false;
		day_slot.GetComponent<Image>().enabled  = false;
		day_slot.GetComponent<Button>().GetComponentInChildren<Text>().enabled = false;
	}

	void setNormalSlot(int numSlot)
	{
		GameObject day_slot = GameObject.Find("Slot_"+ (numSlot));
		day_slot.GetComponent<Button>().enabled = true;
		day_slot.GetComponent<Image>().enabled  = false;
		day_slot.GetComponent<Button>().GetComponentInChildren<Text>().enabled = true;
		day_slot.GetComponent<Button>().GetComponentInChildren<Text>().color = FlatCalendarStyle.color_dayTextNormal;
	}

	void setEventSlot(int numSlot)
	{
		Sprite sprite       = Resources.Load<Sprite>("img/circle_filled");
		GameObject day_slot = GameObject.Find("Slot_"+ (numSlot));
		day_slot.GetComponent<Button>().enabled = true;
		day_slot.GetComponent<Image>().enabled  = true;
		day_slot.GetComponent<Image>().sprite   = sprite;
		day_slot.GetComponent<Image>().color    = FlatCalendarStyle.color_bubbleEvent;
		day_slot.GetComponent<Button>().GetComponentInChildren<Text>().enabled = true;
		day_slot.GetComponent<Button>().GetComponentInChildren<Text>().color = FlatCalendarStyle.color_dayTextEvent;
	}

	void populateAllSlot(int monthNumber, int year)
	{
		// Disable all slots
		disableAllSlot();

		// Update slots
		for (int i = 0; i < currentTime.totalDays; i++)
		{	
			// Put text
			changeTextSlot(i+currentTime.dayOffset+1,""+(i+1));

			// Check if slot event
			if(checkEventExist(currentTime.year,currentTime.month,(i+1)))
				setEventSlot(i+currentTime.dayOffset+1);
			else
				setNormalSlot(i+currentTime.dayOffset+1);
		}
	}

	void changeTextSlot(int numSlot, string text)
	{
		GameObject day_slot = GameObject.Find("Slot_"+numSlot);
		day_slot.GetComponent<Button>().GetComponentInChildren<Text>().text = text;
	}

	int getDayInSlot(int numSlot)
	{
		GameObject day_slot = GameObject.Find("Slot_"+ (numSlot));
		string txt = day_slot.GetComponentInChildren<Text>().text;
		return int.Parse(txt);
	}

	public void markSelectionDay(int day)
	{
		GameObject day_slot = GameObject.Find("Slot_"+ (day+currentTime.dayOffset));

		// Change Image
		if(!checkEventExist(currentTime.year,currentTime.month,day))
		{
			Sprite sprite       = Resources.Load<Sprite>("img/circle_unfilled");
			day_slot.GetComponent<Image>().sprite   = sprite;
			day_slot.GetComponent<Image>().enabled  = true;
			day_slot.GetComponent<Image>().color    = FlatCalendarStyle.color_bubbleSelectionMarker;
			day_slot.GetComponent<Button>().GetComponentInChildren<Text>().color = FlatCalendarStyle.color_dayTextNormal;
			
		}
	
		// Update Text
		label_dayOfWeek.GetComponent<Text>().text = currentTime.dayOfWeek;
		label_dayNumber.GetComponent<Text>().text = "" + currentTime.day;
		CheckNoticias();
	}

	void CheckNoticias()
	{

		if (events_list == null)
			return;

		if (checkEventExist(currentTime.year, currentTime.month, currentTime.day))
		{
			descripcionEvento.text = events_list[currentTime.year][currentTime.month][currentTime.day][0].description;
		}
		else
		{
			descripcionEvento.text = string.Empty;
		}
	}

	void unmarkSelctionDay(int day)
	{
		GameObject day_slot = GameObject.Find("Slot_"+ (day+currentTime.dayOffset));

		// Change Image
		if(!checkEventExist(currentTime.year,currentTime.month,day))
		{
			setNormalSlot(day+currentTime.dayOffset);
		}
	}

	public static bool checkEventExist(int year, int month, int day)
	{
		if(events_list == null)
			return false;

		if(!events_list.ContainsKey(year))
			return false;

		if(!events_list[year].ContainsKey(month))
			return false;

		if(!events_list[year][month].ContainsKey(day))
			return false;

		if(events_list[year][month][day] == null)
			return false;

		if(events_list[year][month][day].Count == 0)
			return false;

		return true;
	}

	void addEventsListener()
	{
		btn_nextMonth.GetComponent<Button>().onClick.AddListener(() => evtListener_NextMonth());
		btn_prevMonth.GetComponent<Button>().onClick.AddListener(() => evtListener_PreviousMonth());
		btn_calendar.GetComponent<Button>().onClick.AddListener(()   => evtListener_GoToNowday());
		for(int i = 0; i < max_day_slots; i++)
			GameObject.Find("Slot_"+(i+1)).GetComponent<Button>().onClick.AddListener(() => evtListener_DaySelected());
	}

	public void setCurrentTime()
	{
		DateTime initTime = TimeManagerScript.Instance.CogerFecha();
		currentTime.day = initTime.Day;
		currentTime.month = initTime.Month;
		currentTime.year = initTime.Year;
		currentTime.dayOfWeek = initTime.DayOfWeek.ToString();
		currentTime.totalDays = System.DateTime.DaysInMonth(currentTime.year,currentTime.month);
		currentTime.dayOffset = getIndexOfFirstSlotInMonth(currentTime.year,currentTime.month);
		
		
	}

	void setCurrentTime(FlatCalendar.TimeObj obj)
	{
		DateTime initTime = TimeManagerScript.Instance.CogerFecha();
		obj.day = initTime.Day;
		obj.month = initTime.Month;
		obj.year = initTime.Year;
		obj.dayOfWeek = initTime.DayOfWeek.ToString();
		
		obj.totalDays = System.DateTime.DaysInMonth(obj.year,obj.month);
		obj.dayOffset = getIndexOfFirstSlotInMonth(obj.year,obj.month);
	}

	public void installDemoData()
	{
		addEvent(2023,6,3,  new EventObj("Event","Pronto los equipos comenzarán con sus partidos de preparación antes de que comienze la competición, veremos como se presenta cada club y lo que esperaremos de ellos durante el año."));
		addEvent(2023,6,7,  new EventObj("Event","Comienzan las partidas importantes, ahora es cuando los clubes deberán dominar a sus rivales ya que el resultado determinará su presencia en proximas competiciones."));
		addEvent(2023,6,19,  new EventObj("Event","Counter-Strike está en boca de todos porque el Source 2 está a punto de llegar y una beta para jugarlo, pero también porque es el juego más jugado de Steam con unos datos que son realmente increíbles."));
		addEvent(2023,7,28,  new EventObj("Event","Hoy es el inicio de una nueva epoca en CS:GO, el nuevo motor grafico ya está aquí, presentamos CS:GO Source 2."));
		addEvent(2023,8,10, new EventObj("Event","ESL confirmó hace un par de días que NAVI, G2 eSports, 9Z y Movistar Riders estarían en el próximo clasificatorio cerrado de ESL Challenger Katowice 2023. Del 20 al 22 de Septiembre, esos cuatro equipos mencionados competirán por ganar uno de los grandes trofeos de CS:GO."));
		addEvent(2023,9,20,  new EventObj("Event","Comienzo del clasificatorio cerrado de ESL Challenger Katowice 2023."));

		//updateCalendar(currentTime.month,currentTime.year);
		//markSelectionDay(currentTime.day);
	}

	public void setUIStyle(int style)
	{
		current_UiStyle = style;
		FlatCalendarStyle.changeUIStyle(current_UiStyle);
	}

	public void addEvent(int year, int month, int day, EventObj ev)
	{
		if(!events_list.ContainsKey(year))
			events_list.Add(year,new Dictionary<int, Dictionary<int,List<EventObj>>>());
		
		if(!events_list[year].ContainsKey(month))
			events_list[year].Add(month,new Dictionary<int, List<EventObj>>());
		
		if(!events_list[year][month].ContainsKey(day))
			events_list[year][month].Add(day,new List<EventObj>());

		events_list[year][month][day].Add(ev);
	}

	public void removeEvent(int year, int month, int day, EventObj ev)
	{
		if(!events_list.ContainsKey(year))
			events_list.Add(year,new Dictionary<int, Dictionary<int,List<EventObj>>>());
		
		if(!events_list[year].ContainsKey(month))
			events_list[year].Add(month,new Dictionary<int, List<EventObj>>());
		
		if(!events_list[year][month].ContainsKey(day))
			events_list[year][month].Add(day,new List<EventObj>());

		if(events_list[year][month][day].Contains(ev))
			events_list[year][month][day].Remove(ev);
	}

	public void removeAllEventOfDay(int year, int month, int day)
	{
		if(!events_list.ContainsKey(year))
			events_list.Add(year,new Dictionary<int, Dictionary<int,List<EventObj>>>());
		
		if(!events_list[year].ContainsKey(month))
			events_list[year].Add(month,new Dictionary<int, List<EventObj>>());
		
		if(!events_list[year][month].ContainsKey(day))
			events_list[year][month].Add(day,new List<EventObj>());

		events_list[year][month][day].Clear();
	}

	public void removeAllCalendarEvents()
	{
		events_list.Clear();
	}

	public static List<EventObj> getEventList(int year, int month, int day)
	{
		List<EventObj> list = new List<EventObj>();

		if(!events_list.ContainsKey(year))
			return list;

		if(!events_list[year].ContainsKey(month))
			return list;

		if(!events_list[year][month].ContainsKey(day))
			return list;

		return events_list[year][month][day];
	}

	void updateUiLabelEvents(int year, int month, int day)
	{
		label_numberEvents.GetComponent<Text>().text = "" + getEventList(year,month,day).Count;
	}
	

	// ================================================
	// =============== BUTTON LISTENERS ===============
	// ================================================
	void evtListener_NextMonth()
	{
		unmarkSelctionDay(currentTime.day);

		currentTime.month = (currentTime.month+1) % 13;
		if(currentTime.month == 0)
		{
			currentTime.year++;
			currentTime.month = 1;
		}


		currentTime.day       = 1;
        currentTime.dayOfWeek = getDayOfWeek(currentTime.year,currentTime.month,currentTime.day);
		currentTime.dayOffset = getIndexOfFirstSlotInMonth(currentTime.year,currentTime.month);
        currentTime.totalDays = System.DateTime.DaysInMonth(currentTime.year, currentTime.month);
  

		updateCalendar(currentTime.month,currentTime.year);

		markSelectionDay(currentTime.day);

		// Update label event
		updateUiLabelEvents(currentTime.year,currentTime.month,currentTime.day);

		// Send Callback
		if(delegate_onmonthchanged != null)
			delegate_onmonthchanged(currentTime);
	}

	void evtListener_PreviousMonth()
	{
		unmarkSelctionDay(currentTime.day);

		currentTime.month = (currentTime.month-1) % 13;
		if(currentTime.month == 0)
		{
			currentTime.year--;
			currentTime.month = 12;
		}

		currentTime.day   = 1;
		currentTime.dayOfWeek = getDayOfWeek(currentTime.year,currentTime.month,currentTime.day);
		currentTime.dayOffset = getIndexOfFirstSlotInMonth(currentTime.year,currentTime.month);
        currentTime.totalDays = System.DateTime.DaysInMonth(currentTime.year, currentTime.month);

		updateCalendar(currentTime.month,currentTime.year);

		markSelectionDay(currentTime.day);

		// Update label event
		updateUiLabelEvents(currentTime.year,currentTime.month,currentTime.day);

		// Send Callback
		if(delegate_onmonthchanged != null)
			delegate_onmonthchanged(currentTime);

	}

	void evtListener_DaySelected()
	{
		// Unmark old slot
		unmarkSelctionDay(currentTime.day);

		// Update current day
		string slot_name             = EventSystem.current.currentSelectedGameObject.name;
		int    slot_position         = int.Parse(slot_name.Substring(5,(slot_name.Length-5)));
		 	   currentTime.day       = getDayInSlot(slot_position);
			   currentTime.dayOfWeek = getDayOfWeek(currentTime.year,currentTime.month,currentTime.day);

		// Mark current slot
		markSelectionDay(currentTime.day);

		// Update label event
		updateUiLabelEvents(currentTime.year,currentTime.month,currentTime.day);

		// Send Callback
		if(delegate_ondayselected != null)
			delegate_ondayselected(currentTime);

		// Send Callback
		if(getEventList(currentTime.year,currentTime.month,currentTime.day).Count > 0)
			if(delegate_oneventselected != null)
				delegate_oneventselected(currentTime,getEventList(currentTime.year,currentTime.month,currentTime.day));
	}

	void evtListener_GoToNowday()
	{
		// Unmark old slot
		unmarkSelctionDay(currentTime.day);

		// Set Current Time
		setCurrentTime();

		// Update Calendar
		updateCalendar(currentTime.month,currentTime.year);

		// Mark Selection Day
		markSelectionDay(currentTime.day);

		// Update label event
		updateUiLabelEvents(currentTime.year,currentTime.month,currentTime.day);

		// Send Callback
		if(delegate_onnowday != null)
			delegate_onnowday(currentTime);
	}

	// =========================================================
	// ================= SET DELEGATE CALLBACKS ================
	// =========================================================

	public void setCallback_OnDaySelected(Delegate_OnDaySelected func)
	{
		delegate_ondayselected = func;
	}

	public void setCallback_OnEventSelected(Delegate_OnEventSelected func)
	{
		delegate_oneventselected = func;
	}

	public void setCallback_OnMonthChanged(Delegate_OnMonthChanged func)
	{
		delegate_onmonthchanged = func;
	}

	public void setCallback_OnNowday(Delegate_OnNowDay func)
	{
		delegate_onnowday = func;
	}
}
