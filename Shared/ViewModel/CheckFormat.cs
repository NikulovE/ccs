#if WPF
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Shared.ViewModel
{
    class CheckFormat
    {
        public static void CheckPhone(TextBox PhoneBox)
        {
            if (PhoneBox.Visibility == Visibility.Visible)
            {
                try
                {
                    ModelView.UIBinding.Default.OutPut = "";
                    if (PhoneBox.Text != "") long.Parse(PhoneBox.Text);

                }
                catch (Exception)
                {
                    try
                    {
                        ModelView.UIBinding.Default.OutPut = ConvertMessages.Message("x50008");
                    }
                    catch (Exception) { }
                }
            }

        }
    }
}
#endif