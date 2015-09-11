//package com.special.ResideMenu;

//import android.app.Activity;
//import android.content.Context;
//import android.content.res.Configuration;
//import android.graphics.Rect;
//import android.util.DisplayMetrics;
//import android.view.*;
//import android.view.animation.AnimationUtils;
//import android.widget.FrameLayout;
//import android.widget.ImageView;
//import android.widget.LinearLayout;
//import android.widget.ScrollView;
//import com.nineoldandroids.animation.Animator;
//import com.nineoldandroids.animation.AnimatorSet;
//import com.nineoldandroids.animation.ObjectAnimator;
//import com.nineoldandroids.view.ViewHelper;

using Android.Animation;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
//import java.util.ArrayList;
//import java.util.List;
using Android.Widget;
using System.Collections.Generic;
using R=AndroidResideMenu.Net.Resource;


namespace Com.Special.ResideMenu
{
    /**
     * User: special
     * Date: 13-12-10
     * Time: 下午10:44
     * Mail: specialcyci@gmail.com
     */
    public class ResideMenu : FrameLayout
    {

        public const int DIRECTION_LEFT = 0;
        public const int DIRECTION_RIGHT = 1;
        private const int PRESSED_MOVE_HORIZONTAL = 2;
        private const int PRESSED_DOWN = 3;
        private const int PRESSED_DONE = 4;
        private const int PRESSED_MOVE_VERTICAL = 5;

        private ImageView imageViewShadow;
        private ImageView imageViewBackground;
        private LinearLayout layoutLeftMenu;
        private LinearLayout layoutRightMenu;
        private ScrollView scrollViewLeftMenu;
        private ScrollView scrollViewRightMenu;
        private ScrollView scrollViewMenu;
        /** Current attaching activity. */
        private Activity activity;
        /** The DecorView of current activity. */
        private ViewGroup viewDecor;
        private TouchDisableView viewActivity;
        /** The flag of menu opening status. */
        private bool misOpened;
        private float shadowAdjustScaleX;
        private float shadowAdjustScaleY;
        /** Views which need stop to intercept touch events. */
        private List<View> ignoredViews;
        private List<ResideMenuItem> leftMenuItems;
        private List<ResideMenuItem> rightMenuItems;
        private DisplayMetrics displayMetrics = new DisplayMetrics();
        private OnMenuListener menuListener;
        private float lastRawX;
        private bool misInIgnoredView = false;
        private int scaleDirection = DIRECTION_LEFT;
        private int pressedState = PRESSED_DOWN;
        private List<int> disabledSwipeDirection = new List<int>();
        // Valid scale factor is between 0.0f and 1.0f.
        private float mScaleValue = 0.5f;

        private bool mUse3D;
        private const int ROTATE_Y_ANGLE = 10;

        public ResideMenu(Context context)
            : base(context)
        {

            initViews(context);
        }

        private void initViews(Context context)
        {
            LayoutInflater inflater = (LayoutInflater)
                    context.GetSystemService(Context.LayoutInflaterService);
            inflater.Inflate(R.Layout.residemenu, this);
            scrollViewLeftMenu = (ScrollView)FindViewById(R.Id.sv_left_menu);
            scrollViewRightMenu = (ScrollView)FindViewById(R.Id.sv_right_menu);
            imageViewShadow = (ImageView)FindViewById(R.Id.iv_shadow);
            layoutLeftMenu = (LinearLayout)FindViewById(R.Id.layout_left_menu);
            layoutRightMenu = (LinearLayout)FindViewById(R.Id.layout_right_menu);
            imageViewBackground = (ImageView)FindViewById(R.Id.iv_background);
            animationListener = new AnimationListenerClass(this);
        }

        //@Override
        protected override bool FitSystemWindows(Rect insets)
        {
            // Applies the content insets to the view's padding, consuming that content (modifying the insets to be 0),
            // and returning true. This behavior is off by default and can be enabled through setFitsSystemWindows(boolean)
            // in api14+ devices.
            this.SetPadding(viewActivity.PaddingLeft + insets.Left, viewActivity.PaddingTop + insets.Top,
                    viewActivity.PaddingRight + insets.Right, viewActivity.PaddingBottom + insets.Bottom);
            insets.Left = insets.Top = insets.Right = insets.Bottom = 0;
            return true;
        }

        /**
         * Set up the activity;
         *
         * @param activity
         */
        public void attachToActivity(Activity activity)
        {
            initValue(activity);
            setShadowAdjustScaleXByOrientation();
            viewDecor.AddView(this, 0);
        }

        private void initValue(Activity activity)
        {
            this.activity = activity;
            leftMenuItems = new List<ResideMenuItem>();
            rightMenuItems = new List<ResideMenuItem>();
            ignoredViews = new List<View>();
            viewDecor = (ViewGroup)activity.Window.DecorView;
            viewActivity = new TouchDisableView(this.activity);

            View mContent = viewDecor.GetChildAt(0);
            viewDecor.RemoveViewAt(0);
            viewActivity.setContent(mContent);
            AddView(viewActivity);

            ViewGroup parent = (ViewGroup)scrollViewLeftMenu.Parent;
            parent.RemoveView(scrollViewLeftMenu);
            parent.RemoveView(scrollViewRightMenu);
        }

        private void setShadowAdjustScaleXByOrientation()
        {
            Android.Content.Res.Orientation orientation = Resources.Configuration.Orientation;
            if (orientation == Android.Content.Res.Orientation.Landscape)
            {
                shadowAdjustScaleX = 0.034f;
                shadowAdjustScaleY = 0.12f;
            }
            else if (orientation == Android.Content.Res.Orientation.Portrait)
            {
                shadowAdjustScaleX = 0.06f;
                shadowAdjustScaleY = 0.07f;
            }
        }

        /**
         * Set the background image of menu;
         *
         * @param imageResource
         */
        public void setBackground(int imageResource)
        {
            imageViewBackground.SetImageResource(imageResource);
        }

        /**
         * The visibility of the shadow under the activity;
         *
         * @param isVisible
         */
        public void setShadowVisible(bool isVisible)
        {
            if (isVisible)
                imageViewShadow.SetBackgroundResource(R.Drawable.shadow);
            else
                imageViewShadow.SetBackgroundResource(0);
        }

        /**
         * Add a single item to the left menu;
         *
         * WARNING: It will be removed from v2.0.
         * @param menuItem
         */
        //@Deprecated

        public void addMenuItem(ResideMenuItem menuItem)
        {
            this.leftMenuItems.Add(menuItem);
            layoutLeftMenu.AddView(menuItem);
        }

        /**
         * Add a single items;
         *
         * @param menuItem
         * @param direction
         */
        public void addMenuItem(ResideMenuItem menuItem, int direction)
        {
            if (direction == DIRECTION_LEFT)
            {
                this.leftMenuItems.Add(menuItem);
                layoutLeftMenu.AddView(menuItem);
            }
            else
            {
                this.rightMenuItems.Add(menuItem);
                layoutRightMenu.AddView(menuItem);
            }
        }

        /**
         * WARNING: It will be removed from v2.0.
         * @param menuItems
         */
        //@Deprecated
        public void setMenuItems(List<ResideMenuItem> menuItems)
        {
            this.leftMenuItems = menuItems;
            rebuildMenu();
        }

        /**
         * Set menu items by a array;
         *
         * @param menuItems
         * @param direction
         */
        public void setMenuItems(List<ResideMenuItem> menuItems, int direction)
        {
            if (direction == DIRECTION_LEFT)
                this.leftMenuItems = menuItems;
            else
                this.rightMenuItems = menuItems;
            rebuildMenu();
        }

        private void rebuildMenu()
        {
            layoutLeftMenu.RemoveAllViews();
            layoutRightMenu.RemoveAllViews();
            foreach (ResideMenuItem leftMenuItem in leftMenuItems)
                layoutLeftMenu.AddView(leftMenuItem);
            foreach (ResideMenuItem rightMenuItem in rightMenuItems)
                layoutRightMenu.AddView(rightMenuItem);
        }

        /**
         * WARNING: It will be removed from v2.0.
         * @return
         */
        //@Deprecated
        public List<ResideMenuItem> getMenuItems()
        {
            return leftMenuItems;
        }

        /**
         * Return instances of menu items;
         *
         * @return
         */
        public List<ResideMenuItem> getMenuItems(int direction)
        {
            if (direction == DIRECTION_LEFT)
                return leftMenuItems;
            else
                return rightMenuItems;
        }

        /**
         * If you need to do something on closing or opening menu,
         * set a listener here.
         *
         * @return
         */
        public void setMenuListener(OnMenuListener menuListener)
        {
            this.menuListener = menuListener;
        }


        public OnMenuListener getMenuListener()
        {
            return menuListener;
        }

        /**
         * Show the menu;
         */
        public void openMenu(int direction)
        {

            setScaleDirection(direction);

            misOpened = true;
            AnimatorSet scaleDown_activity = buildScaleDownAnimation(viewActivity, mScaleValue, mScaleValue);
            AnimatorSet scaleDown_shadow = buildScaleDownAnimation(imageViewShadow,
                    mScaleValue + shadowAdjustScaleX, mScaleValue + shadowAdjustScaleY);
            AnimatorSet alpha_menu = buildMenuAnimation(scrollViewMenu, 1.0f);
            scaleDown_shadow.AddListener(animationListener);
            scaleDown_activity.PlayTogether(scaleDown_shadow);
            scaleDown_activity.PlayTogether(alpha_menu);
            scaleDown_activity.Start();
        }

        /**
         * Close the menu;
         */
        public void closeMenu()
        {

            misOpened = false;
            AnimatorSet scaleUp_activity = buildScaleUpAnimation(viewActivity, 1.0f, 1.0f);
            AnimatorSet scaleUp_shadow = buildScaleUpAnimation(imageViewShadow, 1.0f, 1.0f);
            AnimatorSet alpha_menu = buildMenuAnimation(scrollViewMenu, 0.0f);
            scaleUp_activity.AddListener(animationListener);
            scaleUp_activity.PlayTogether(scaleUp_shadow);
            scaleUp_activity.PlayTogether(alpha_menu);
            scaleUp_activity.Start();
        }

        //@Deprecated
        public void setDirectionDisable(int direction)
        {
            disabledSwipeDirection.Add(direction);
        }

        public void setSwipeDirectionDisable(int direction)
        {
            disabledSwipeDirection.Add(direction);
        }

        private bool isInDisableDirection(int direction)
        {
            return disabledSwipeDirection.Contains(direction);
        }

        private void setScaleDirection(int direction)
        {

            int screenWidth = getScreenWidth();
            float pivotX;
            float pivotY = getScreenHeight() * 0.5f;

            if (direction == DIRECTION_LEFT)
            {
                scrollViewMenu = scrollViewLeftMenu;
                pivotX = screenWidth * 1.5f;
            }
            else
            {
                scrollViewMenu = scrollViewRightMenu;
                pivotX = screenWidth * -0.5f;
            }

            //ViewHelper.setPivotX(viewActivity, pivotX);
            //ViewHelper.setPivotY(viewActivity, pivotY);
            //ViewHelper.setPivotX(imageViewShadow, pivotX);
            //ViewHelper.setPivotY(imageViewShadow, pivotY);
            viewActivity.PivotX=pivotX;
            viewActivity.PivotY=pivotY;
            imageViewShadow.PivotX=pivotX;
            imageViewShadow.PivotY=pivotY;

            scaleDirection = direction;
        }

        /**
         * return the flag of menu status;
         *
         * @return
         */
        public bool isOpened()
        {
            return misOpened;
        }
        //private IOnClickListener viewActivityOnClickListener;
        //private OnClickListener viewActivityOnClickListener = new OnClickListener() {
        //    @Override
        //    public void onClick(View view) {
        //        if (isOpened()) closeMenu();
        //    }
        //};

        class AnimationListenerClass : Java.Lang.Object, Animator.IAnimatorListener
        {
            private ResideMenu minst;

            public AnimationListenerClass(ResideMenu minstance)
            {
                minst = minstance;
            }

            public void OnAnimationCancel(Animator animation)
            {
                
            }

            public void OnAnimationEnd(Animator animation)
            {
                if (minst.isOpened())
                {
                    minst.viewActivity.setTouchDisable(true);
                    //minst.viewActivity.SetOnClickListener(viewActivityOnClickListener);
					minst.viewActivity.Click+=delegate{
					if (minst.isOpened()) minst.closeMenu();
                };
                }
                else
                {
                    minst.viewActivity.setTouchDisable(false);
                    minst.viewActivity.SetOnClickListener(null);
                    minst.hideScrollViewMenu(minst.scrollViewLeftMenu);
                    minst.hideScrollViewMenu(minst.scrollViewRightMenu);
                    if (minst.menuListener != null)
                        minst.menuListener.closeMenu();
                }
            }

            public void OnAnimationRepeat(Animator animation)
            {
                 
            }

            public void OnAnimationStart(Animator animation)
            {
                if (minst.isOpened()){
                    minst.showScrollViewMenu(minst.scrollViewMenu);
                    if (minst.menuListener != null)
                        minst.menuListener.openMenu();
                }
            }
        }

        private Animator.IAnimatorListener animationListener;//=new AnimationListenerClass(this);
        //private Animator.AnimatorListener animationListener = new Animator.AnimatorListener() {
        //    @Override
        //    public void onAnimationStart(Animator animation) {
        //        if (isOpened()){
        //            showScrollViewMenu(scrollViewMenu);
        //            if (menuListener != null)
        //                menuListener.openMenu();
        //        }
        //    }

        //    @Override
        //    public void onAnimationEnd(Animator animation) {
        //        // reset the view;
        //        if(isOpened()){
        //            viewActivity.setTouchDisable(true);
        //            viewActivity.setOnClickListener(viewActivityOnClickListener);
        //        }else{
        //            viewActivity.setTouchDisable(false);
        //            viewActivity.setOnClickListener(null);
        //            hideScrollViewMenu(scrollViewLeftMenu);
        //            hideScrollViewMenu(scrollViewRightMenu);
        //            if (menuListener != null)
        //                menuListener.closeMenu();
        //        }
        //    }

        //    @Override
        //    public void onAnimationCancel(Animator animation) {

        //    }

        //    @Override
        //    public void onAnimationRepeat(Animator animation) {

        //    }
        //};

        /**
         * A helper method to build scale down animation;
         *
         * @param target
         * @param targetScaleX
         * @param targetScaleY
         * @return
         */
        private AnimatorSet buildScaleDownAnimation(View target, float targetScaleX, float targetScaleY)
        {

            AnimatorSet scaleDown = new AnimatorSet();
            scaleDown.PlayTogether(
                    ObjectAnimator.OfFloat(target, "scaleX", targetScaleX),
                    ObjectAnimator.OfFloat(target, "scaleY", targetScaleY)
            );

            if (mUse3D)
            {
                int angle = scaleDirection == DIRECTION_LEFT ? -ROTATE_Y_ANGLE : ROTATE_Y_ANGLE;
                scaleDown.PlayTogether(ObjectAnimator.OfFloat(target, "rotationY", angle));
            }

            scaleDown.SetInterpolator(AnimationUtils.LoadInterpolator(activity,
                    Android.Resource.Animation.DecelerateInterpolator));
            scaleDown.SetDuration(250);
            return scaleDown;
        }

        /**
         * A helper method to build scale up animation;
         *
         * @param target
         * @param targetScaleX
         * @param targetScaleY
         * @return
         */
        private AnimatorSet buildScaleUpAnimation(View target, float targetScaleX, float targetScaleY)
        {

            AnimatorSet scaleUp = new AnimatorSet();
            scaleUp.PlayTogether(
                    ObjectAnimator.OfFloat(target, "scaleX", targetScaleX),
                    ObjectAnimator.OfFloat(target, "scaleY", targetScaleY)
            );

            if (mUse3D)
            {
                scaleUp.PlayTogether(ObjectAnimator.OfFloat(target, "rotationY", 0));
            }

            scaleUp.SetDuration(250);
            return scaleUp;
        }

        private AnimatorSet buildMenuAnimation(View target, float alpha)
        {

            AnimatorSet alphaAnimation = new AnimatorSet();
            alphaAnimation.PlayTogether(
                    ObjectAnimator.OfFloat(target, "alpha", alpha)
            );

            alphaAnimation.SetDuration(250);
            return alphaAnimation;
        }

        /**
         * If there were some view you don't want reside menu
         * to intercept their touch event, you could add it to
         * ignored views.
         *
         * @param v
         */
        public void addIgnoredView(View v)
        {
            ignoredViews.Add(v);
        }

        /**
         * Remove a view from ignored views;
         * @param v
         */
        public void removeIgnoredView(View v)
        {
            ignoredViews.Remove(v);
        }

        /**
         * Clear the ignored view list;
         */
        public void clearIgnoredViewList()
        {
            ignoredViews.Clear();
        }

        /**
         * If the motion event was relative to the view
         * which in ignored view list,return true;
         *
         * @param ev
         * @return
         */
        private bool isInIgnoredView(MotionEvent ev)
        {
            Rect rect = new Rect();
            foreach (View v in ignoredViews)
            {
                v.GetGlobalVisibleRect(rect);
                if (rect.Contains((int)ev.GetX(), (int)ev.GetY()))
                    return true;
            }
            return false;
        }

        private void setScaleDirectionByRawX(float currentRawX)
        {
            if (currentRawX < lastRawX)
                setScaleDirection(DIRECTION_RIGHT);
            else
                setScaleDirection(DIRECTION_LEFT);
        }

        private float getTargetScale(float currentRawX)
        {
            float scaleFloatX = ((currentRawX - lastRawX) / getScreenWidth()) * 0.75f;
            scaleFloatX = scaleDirection == DIRECTION_RIGHT ? -scaleFloatX : scaleFloatX;

            float targetScale = viewActivity.ScaleX-scaleFloatX;// ViewHelper.getScaleX(viewActivity) - scaleFloatX;
            targetScale = targetScale > 1.0f ? 1.0f : targetScale;
            targetScale = targetScale < 0.5f ? 0.5f : targetScale;
            return targetScale;
        }

        private float lastActionDownX, lastActionDownY;

        //@Override
        public override bool DispatchTouchEvent(MotionEvent ev)
        {
            float currentActivityScaleX = viewActivity.ScaleX;//ViewHelper.getScaleX(viewActivity);
            if (currentActivityScaleX == 1.0f)
                setScaleDirectionByRawX(ev.RawX);

            switch (ev.Action)
            {
                case MotionEventActions.Down:
                    lastActionDownX = ev.GetX();
                    lastActionDownY = ev.GetY();
                    misInIgnoredView = this.isInIgnoredView(ev) && !isOpened();
                    pressedState = PRESSED_DOWN;
                    break;

                case MotionEventActions.Move:
                    if (misInIgnoredView || isInDisableDirection(scaleDirection))
                        break;

                    if (pressedState != PRESSED_DOWN &&
                            pressedState != PRESSED_MOVE_HORIZONTAL)
                        break;

                    int xOffset = (int)(ev.GetX() - lastActionDownX);
                    int yOffset = (int)(ev.GetY() - lastActionDownY);

                    if (pressedState == PRESSED_DOWN)
                    {
                        if (yOffset > 25 || yOffset < -25)
                        {
                            pressedState = PRESSED_MOVE_VERTICAL;
                            break;
                        }
                        if (xOffset < -50 || xOffset > 50)
                        {
                            pressedState = PRESSED_MOVE_HORIZONTAL;
                            ev.Action = MotionEventActions.Cancel;
                        }
                    }
                    else if (pressedState == PRESSED_MOVE_HORIZONTAL)
                    {
                        if (currentActivityScaleX < 0.95)
                            showScrollViewMenu(scrollViewMenu);

                        float targetScale = getTargetScale(ev.RawX);
                        //ViewHelper.setScaleX(viewActivity, targetScale);
                        //ViewHelper.setScaleY(viewActivity, targetScale);
                        //ViewHelper.setScaleX(imageViewShadow, targetScale + shadowAdjustScaleX);
                        //ViewHelper.setScaleY(imageViewShadow, targetScale + shadowAdjustScaleY);
                        //ViewHelper.setAlpha(scrollViewMenu, (1 - targetScale) * 2.0f);

                        viewActivity.ScaleX=targetScale;
                        viewActivity.ScaleY=targetScale;
                        imageViewShadow.ScaleX=targetScale + shadowAdjustScaleX;
                        imageViewShadow.ScaleY=targetScale + shadowAdjustScaleY;
                        imageViewShadow.Alpha= (1 - targetScale) * 2.0f;

                        lastRawX = ev.RawX;
                        return true;
                    }

                    break;

                case MotionEventActions.Up:

                    if (misInIgnoredView) break;
                    if (pressedState != PRESSED_MOVE_HORIZONTAL) break;

                    pressedState = PRESSED_DONE;
                    if (isOpened())
                    {
                        if (currentActivityScaleX > 0.56f)
                            closeMenu();
                        else
                            openMenu(scaleDirection);
                    }
                    else
                    {
                        if (currentActivityScaleX < 0.94f)
                        {
                            openMenu(scaleDirection);
                        }
                        else
                        {
                            closeMenu();
                        }
                    }

                    break;

            }
            lastRawX = ev.RawX;
            return base.DispatchTouchEvent(ev);
        }

        public int getScreenHeight()
        {
            activity.WindowManager.DefaultDisplay.GetMetrics(displayMetrics);
            return displayMetrics.HeightPixels;
        }

        public int getScreenWidth()
        {
            activity.WindowManager.DefaultDisplay.GetMetrics(displayMetrics);
            return displayMetrics.WidthPixels;
        }

        public void setScaleValue(float scaleValue)
        {
            this.mScaleValue = scaleValue;
        }

        public void setUse3D(bool use3D)
        {
            mUse3D = use3D;
        }

        public interface OnMenuListener
        {

            /**
             * This method will be called at the finished time of opening menu animations.
             */
            void openMenu();

            /**
             * This method will be called at the finished time of closing menu animations.
             */
            void closeMenu();
        }

        private void showScrollViewMenu(ScrollView scrollViewMenu)
        {
            if (scrollViewMenu != null && scrollViewMenu.Parent == null)
            {
                AddView(scrollViewMenu);
            }
        }

        private void hideScrollViewMenu(ScrollView scrollViewMenu)
        {
            if (scrollViewMenu != null && scrollViewMenu.Parent != null)
            {
                RemoveView(scrollViewMenu);
            }
        }
    }
}