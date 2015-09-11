//package com.special.ResideMenu;

//import android.content.Context;
//import android.view.LayoutInflater;
//import android.widget.ImageView;
//import android.widget.LinearLayout;
//import android.widget.TextView;
//import com.special.ResideMenu.R;

using Android.Content;
using Android.Views;
using Android.Widget;
using R=AndroidResideMenu.Net.Resource;

namespace Com.Special.ResideMenu
{
    /**
     * User: special
     * Date: 13-12-10
     * Time: 下午11:05
     * Mail: specialcyci@gmail.com
     */
    public class ResideMenuItem : LinearLayout
    {

        /** menu item  icon  */
        private ImageView iv_icon;
        /** menu item  title */
        private TextView tv_title;

        public ResideMenuItem(Context context)
            : base(context)
        {

            initViews(context);
        }

        public ResideMenuItem(Context context, int icon, int title)
            : base(context)
        {

            initViews(context);
            iv_icon.SetImageResource(icon);
            tv_title.SetText(title);
        }

        public ResideMenuItem(Context context, int icon, string title)
            : base(context)
        {

            initViews(context);
            iv_icon.SetImageResource(icon);
            tv_title.Text = title;
        }

        private void initViews(Context context)
        {
            LayoutInflater inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            inflater.Inflate(R.Layout.residemenu_item, this);
            iv_icon = (ImageView)FindViewById(R.Id.iv_icon);
            tv_title = (TextView)FindViewById(R.Id.tv_title);
        }

        /**
         * set the icon color;
         *
         * @param icon
         */
        public void setIcon(int icon)
        {
            iv_icon.SetImageResource(icon);
        }

        /**
         * set the title with resource
         * ;
         * @param title
         */
        public void setTitle(int title)
        {
            tv_title.SetText(title);
        }

        /**
         * set the title with string;
         *
         * @param title
         */
        public void setTitle(string title)
        {
            tv_title.Text = title;
        }
    }
}