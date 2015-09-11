//package com.special.ResideMenuDemo;

//import android.os.Bundle;
//import android.support.v4.app.Fragment;
//import android.view.LayoutInflater;
//import android.view.View;
//import android.view.ViewGroup;
//import android.widget.FrameLayout;
//import com.special.ResideMenu.ResideMenu;


using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using RResideMenu=Com.Special.ResideMenu.ResideMenu;
using R=ResideMenuDemo.Net.Resource;
using Android.Widget;

namespace Com.Special.ResideMenuDemo
{
    /**
     * User: special
     * Date: 13-12-22
     * Time: 下午1:33
     * Mail: specialcyci@gmail.com
     */
    public class HomeFragment : Fragment
    {

        private View parentView;
        private RResideMenu resideMenu;

        //@Override
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            parentView = inflater.Inflate(R.Layout.home, container, false);
            setUpViews();
            return parentView;
        }

        private void setUpViews()
        {
            MenuActivity parentActivity = (MenuActivity)Activity;
            resideMenu = parentActivity.getResideMenu();

            //parentView.findViewById(R.id.btn_open_menu).setOnClickListener(new View.OnClickListener() {
            //    @Override
            //    public void onClick(View view) {
            //        resideMenu.openMenu(ResideMenu.DIRECTION_LEFT);
            //    }
            //});
            parentView.FindViewById(R.Id.btn_open_menu).Click += delegate
            {
                resideMenu.openMenu(RResideMenu.DIRECTION_LEFT);
            };
            // add gesture operation's ignored views
            FrameLayout ignored_view = (FrameLayout)parentView.FindViewById(R.Id.ignored_view);
            resideMenu.addIgnoredView(ignored_view);
        }

    }
}