using System;
using System.Collections.Generic;
using System.Text;
using Android.Widget;
using Android.Graphics;
using Android.Content;
using Android.Content.Res;

namespace Shared.View
{
    class General
    {

        public static void DisableButton(Button But)
        {
            But.Enabled = false;

        }

        public static void EnableButton(Button But)
        {
            But.Enabled = true;

        }

        public static void SetSegoeMDLFont(Button Element, AssetManager context) {
            Typeface font = Typeface.CreateFromAsset(context, "fonts/SEGMDL2.TTF");
            Element.SetTypeface(font, TypefaceStyle.Normal);
        }

        public static void SetSegoeMDLFont(TextView Element, AssetManager context)
        {
            Typeface font = Typeface.CreateFromAsset(context, "fonts/SEGMDL2.TTF");
            Element.SetTypeface(font, TypefaceStyle.Normal);
        }

        public static void inLoading(ProgressBar bar, TextView Output)
        {
            bar.Indeterminate = true;
            Output.Text = "";
        }

        public static void outLoading(ProgressBar bar)
        {
            bar.Indeterminate = false;
        }

    }

}
