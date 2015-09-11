//package com.special.ResideMenuDemo;

//import android.os.Bundle;
//import android.support.v4.app.Fragment;
//import android.view.LayoutInflater;
//import android.view.View;
//import android.view.ViewGroup;
//import android.widget.*;

//import java.util.ArrayList;
//import java.util.List;


using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using R=ResideMenuDemo.Net.Resource;

namespace Com.Special.ResideMenuDemo
{
    /**
     * User: special
     * Date: 13-12-22
     * Time: 下午3:26
     * Mail: specialcyci@gmail.com
     */
    public class CalendarFragment : Fragment
    {

        private View parentView;
        private ListView listView;

        //@Override
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            parentView = inflater.Inflate(R.Layout.calendar, container, false);
            listView = (ListView)parentView.FindViewById(R.Id.listView);
            initView();
            return parentView;
        }

        private void initView()
        {
            ArrayAdapter<string> arrayAdapter = new ArrayAdapter<string>(
                    this.Activity,
                    Android.Resource.Layout.SimpleListItem1,
                    getCalendarData());
            listView.SetAdapter(arrayAdapter);
            //listView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            //    @Override
            //    public void onItemClick(AdapterView<?> adapterView, View view, int i, long l) {
            //        Toast.makeText(getActivity(), "Clicked item!", Toast.LENGTH_LONG).show();
            //    }
            //});

            listView.ItemClick += delegate
            {
                Toast.MakeText(this.Activity, "Clicked item!", ToastLength.Long).Show();
            };
        }

        private List<string> getCalendarData()
        {
            List<string> calendarList = new List<string>();
            calendarList.Add("New Year's Day");
            calendarList.Add("St. Valentine's Day");
            calendarList.Add("Easter Day");
            calendarList.Add("April Fool's Day");
            calendarList.Add("Mother's Day");
            calendarList.Add("Memorial Day");
            calendarList.Add("National Flag Day");
            calendarList.Add("Father's Day");
            calendarList.Add("Independence Day");
            calendarList.Add("Labor Day");
            calendarList.Add("Columbus Day");
            calendarList.Add("Halloween");
            calendarList.Add("All Soul's Day");
            calendarList.Add("Veterans Day");
            calendarList.Add("Thanksgiving Day");
            calendarList.Add("Election Day");
            calendarList.Add("Forefather's Day");
            calendarList.Add("Christmas Day");
            return calendarList;
        }
    }
}
