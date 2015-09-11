//package com.special.ResideMenuDemo;

//import android.os.Bundle;
//import android.support.v4.app.Fragment;
//import android.view.LayoutInflater;
//import android.view.View;
//import android.view.ViewGroup;


using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using ResideMenuDemo.Net;
namespace Com.Special.ResideMenuDemo
{
    /**
     * User: special
     * Date: 13-12-22
     * Time: 下午3:28
     * Mail: specialcyci@gmail.com
     */
    public class SettingsFragment : Fragment
    {

        //@Override
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.settings, container, false);
        }

    }
}
