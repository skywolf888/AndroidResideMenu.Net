//package com.special.ResideMenuDemo;

//import android.os.Bundle;
//import android.support.v4.app.Fragment;
//import android.support.v4.app.FragmentActivity;
//import android.support.v4.app.FragmentTransaction;
//import android.view.MotionEvent;
//import android.view.View;
//import android.widget.Toast;
//import com.special.ResideMenu.ResideMenu;
//import com.special.ResideMenu.ResideMenuItem;

using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using RResideMenu=Com.Special.ResideMenu.ResideMenu;
using R=ResideMenuDemo.Net.Resource;
using Android.Widget;
using Com.Special.ResideMenu;
using Com.Special.ResideMenuDemo;
using Android.App;


namespace Com.Special.ResideMenuDemo
{

    [Activity(Label = "MenuActivity.Net", MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.Light.NoTitleBar")]
    public class MenuActivity : FragmentActivity, View.IOnClickListener, RResideMenu.OnMenuListener
    {

        private RResideMenu resideMenu;
        private MenuActivity mContext;
        private ResideMenuItem itemHome;
        private ResideMenuItem itemProfile;
        private ResideMenuItem itemCalendar;
        private ResideMenuItem itemSettings;

        /**
         * Called when the activity is first created.
         */
        //@Override
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(R.Layout.Main);
            mContext = this;
            setUpMenu();
            if (savedInstanceState == null)
                changeFragment(new HomeFragment());
        }

        private void setUpMenu()
        {

            // attach to current activity;
            resideMenu = new RResideMenu(this);
            resideMenu.setUse3D(true);
            resideMenu.setBackground(R.Drawable.menu_background);
            resideMenu.attachToActivity(this);
            resideMenu.setMenuListener(this);
            //valid scale factor is between 0.0f and 1.0f. leftmenu'width is 150dip. 
            resideMenu.setScaleValue(0.6f);

            // create menu items;
            itemHome = new ResideMenuItem(this, R.Drawable.icon_home, "Home");
            itemProfile = new ResideMenuItem(this, R.Drawable.icon_profile, "Profile");
            itemCalendar = new ResideMenuItem(this, R.Drawable.icon_calendar, "Calendar");
            itemSettings = new ResideMenuItem(this, R.Drawable.icon_settings, "Settings");

            itemHome.SetOnClickListener(this);
            itemProfile.SetOnClickListener(this);
            itemCalendar.SetOnClickListener(this);
            itemSettings.SetOnClickListener(this);

            resideMenu.addMenuItem(itemHome, RResideMenu.DIRECTION_LEFT);
            resideMenu.addMenuItem(itemProfile, RResideMenu.DIRECTION_LEFT);
            resideMenu.addMenuItem(itemCalendar, RResideMenu.DIRECTION_RIGHT);
            resideMenu.addMenuItem(itemSettings, RResideMenu.DIRECTION_RIGHT);

            // You can disable a direction by setting ->
            // resideMenu.setSwipeDirectionDisable(ResideMenu.DIRECTION_RIGHT);

            //FindViewById(R.id.title_bar_left_menu).setOnClickListener(new View.OnClickListener() {
            //    @Override
            //    public void onClick(View view) {
            //        resideMenu.openMenu(ResideMenu.DIRECTION_LEFT);
            //    }
            //});
            FindViewById(R.Id.title_bar_left_menu).Click += delegate
            {
                resideMenu.openMenu(RResideMenu.DIRECTION_LEFT);
            };

            //findViewById(R.id.title_bar_right_menu).setOnClickListener(new View.OnClickListener() {
            //    @Override
            //    public void onClick(View view) {
            //        resideMenu.openMenu(ResideMenu.DIRECTION_RIGHT);
            //    }
            //});
            FindViewById(R.Id.title_bar_right_menu).Click += delegate
            {
                resideMenu.openMenu(RResideMenu.DIRECTION_RIGHT);
            };
        }

        //@Override
        public override bool DispatchTouchEvent(MotionEvent ev)
        {
            return resideMenu.DispatchTouchEvent(ev);
        }

        //@Override
        public void OnClick(View view)
        {

            if (view == itemHome)
            {
                changeFragment(new HomeFragment());
            }
            else if (view == itemProfile)
            {
                changeFragment(new ProfileFragment());
            }
            else if (view == itemCalendar)
            {
                changeFragment(new CalendarFragment());
            }
            else if (view == itemSettings)
            {
                changeFragment(new SettingsFragment());
            }

            resideMenu.closeMenu();
        }

        //private ResideMenu.OnMenuListener menuListener = new ResideMenu.OnMenuListener() {
        //    @Override
        //    public void openMenu() {
        //        Toast.makeText(mContext, "Menu is opened!", Toast.LENGTH_SHORT).show();
        //    }

        //    @Override
        //    public void closeMenu() {
        //        Toast.makeText(mContext, "Menu is closed!", Toast.LENGTH_SHORT).show();
        //    }
        //};

        private void changeFragment(Android.Support.V4.App.Fragment targetFragment)
        {
            resideMenu.clearIgnoredViewList();
            SupportFragmentManager
                    .BeginTransaction()
                    .Replace(R.Id.main_fragment, targetFragment, "fragment")
                    .SetTransitionStyle(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade)
                    .Commit();
        }

        // What good method is to access resideMenu？
        public RResideMenu getResideMenu()
        {
            return resideMenu;
        }

        void RResideMenu.OnMenuListener.openMenu()
        {
            Toast.MakeText(mContext, "Menu is opened!", ToastLength.Short).Show();
        }

        void RResideMenu.OnMenuListener.closeMenu()
        {
            Toast.MakeText(mContext, "Menu is closed!", ToastLength.Short).Show();
        }
    }
}