//package com.special.ResideMenu;



//import android.content.Context;
//import android.util.AttributeSet;
//import android.view.MotionEvent;
//import android.view.View;
//import android.view.ViewGroup;


using Android.Content;
using Android.Util;
using Android.Views;
namespace Com.Special.ResideMenu
{
    /**
     * Created by thonguyen on 15/4/14.
     */
    class TouchDisableView : ViewGroup
    {

        private View mContent;

        //	private int mMode;
        private bool mTouchDisabled = false;

        public TouchDisableView(Context context)
            : this(context, null)
        {

        }

        public TouchDisableView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {

        }

        public void setContent(View v)
        {
            if (mContent != null)
            {
                this.RemoveView(mContent);
            }
            mContent = v;
            AddView(mContent);
        }

        public View getContent()
        {
            return mContent;
        }

        //@Override
        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {

            int width = GetDefaultSize(0, widthMeasureSpec);
            int height = GetDefaultSize(0, heightMeasureSpec);
            SetMeasuredDimension(width, height);

            int contentWidth = GetChildMeasureSpec(widthMeasureSpec, 0, width);
            int contentHeight = GetChildMeasureSpec(heightMeasureSpec, 0, height);
            mContent.Measure(contentWidth, contentHeight);
        }

        //@Override
        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            int width = r - l;
            int height = b - t;
            mContent.Layout(0, 0, width, height);
        }

        //@Override
        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            return mTouchDisabled;
        }

        public void setTouchDisable(bool disableTouch)
        {
            mTouchDisabled = disableTouch;
        }

        bool isTouchDisabled()
        {
            return mTouchDisabled;
        }
    }
}