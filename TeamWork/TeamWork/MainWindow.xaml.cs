using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Birthday0
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        int? year = null;
        int month, day;


        private void InputYear_TextChanged(object sender, TextChangedEventArgs e)
        {
            year = int.Parse(this.InputYear.Text);
        }

        private void Assure_Click(object sender, RoutedEventArgs e)
        {
            month = int.Parse(ChooseMonth.Text);
            day = int.Parse(this.ChooseDay.Text);

            //计算距离下次生日的距离
            String[] dateTimeNowStr = DateToString(DateTime.Now);
            int[] dateTimeNowInt = StringToInt(dateTimeNowStr);

            DateTime dateTimeInput = new DateTime(dateTimeNowInt[0], month, day);

            if (CompareDate(DateTime.Now, dateTimeInput))
                NextBirthday.Text = (getYearDays(DateTime.Now.Year) - DateTime.Now.DayOfYear + dateTimeInput.DayOfYear).ToString();
            else
                NextBirthday.Text = (dateTimeInput.DayOfYear - DateTime.Now.DayOfYear).ToString();

            //判断星座
            int[] date = { 21, 19, 20, 20, 21, 21, 22, 23, 23, 23, 22, 21 };
            string[] Constellation = { "摩羯座", "水瓶座", "双鱼座", "白羊座", "金牛座", "双子座", "巨蟹座", "狮子座", "处女座", "天秤座", "天蝎座", "射手座", "摩羯座" };
            if (day <= date[month - 1])
            {
                ConstellationOutput.Text = Constellation[month - 1];
            }
            else ConstellationOutput.Text = Constellation[month];


            if (year != null)
            {
                int yearnew = Convert.ToInt32(year);
                DateTime dateTimeInputReal = new DateTime(yearnew, month, day);

                HasBorn.Text = CaculateDaysDistance(dateTimeInputReal, DateTime.Now).ToString();


            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int dayCount = Convert.ToInt32(多少天.Text);
            if (year != null)
            {
                int yearnew = Convert.ToInt32(year);
                DateTime dateTimeInputReal = new DateTime(yearnew, month, day);

                int a = yearnew + dayCount / 365, b = month, c = day;
                while (true)
                {
                    DateTime dateTime = new DateTime(a, b, c);
                    if (CaculateDaysDistance(dateTimeInputReal, dateTime) == dayCount)
                        break;

                    if (c < getMonthDays(a, b))
                        c++;
                    else if (b < 12)
                    {
                        b++;
                        c = 1;
                    }
                    else
                    {
                        a++;
                        b = 1;
                        c = 1;
                    }
                }
                万天计划.Text = a + "." + b + "." + c;
            }
        }

        //输入年月判断该月天数
        static int getMonthDays(int year, int month)
        {
            switch (month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    return 31;
                case 4:
                case 6:
                case 9:
                case 11:
                    return 30;
                default:
                    {
                        if (DateTime.IsLeapYear(year))
                            return 29;
                        else return 28;
                    }

            }
        }

        //日期比较在一年中谁先谁后
        static bool CompareDate(DateTime dateTime1, DateTime dateTime2)
        {
            String[] dateTimeStr1 = DateToString(dateTime1);
            String[] dateTimeStr2 = DateToString(dateTime2);
            int[] dateTimeInt1 = StringToInt(dateTimeStr1);
            int[] dateTimeInt2 = StringToInt(dateTimeStr2);
            int a = dateTimeInt1[1], b = dateTimeInt1[2];
            int c = dateTimeInt2[1], d = dateTimeInt2[2];

            if (a > c)
            {
                return true;
            }
            else if (a == c && b > d)
            {
                return true;
            }
            else return false;
        }


        //提取日期为字符串数组
        static String[] DateToString(DateTime dateTime)
        {
            String dateTimeStr = dateTime.ToShortDateString();
            String[] dateTimeStrs = dateTimeStr.Split('/');
            return dateTimeStrs;
        }

        //String转int
        static int[] StringToInt(String[] str)
        {
            int length = str.Length;
            int[] strToInt = new int[length];
            for (int i = 0; i < length; i++)
            {
                strToInt[i] = int.Parse(str[i]);
            }
            return strToInt;
        }

        //计算两个日期之间的间隔
        static int CaculateDaysDistance(DateTime dateTime1, DateTime dateTime2)
        {
            String[] dateTimeStr1 = DateToString(dateTime1);
            String[] dateTimeStr2 = DateToString(dateTime2);
            int[] dateTimeInt1 = StringToInt(dateTimeStr1);
            int[] dateTimeInt2 = StringToInt(dateTimeStr2);

            if (dateTimeInt1[0] == dateTimeInt2[0])
            {
                return dateTime2.DayOfYear - dateTime1.DayOfYear;
            }
            else
            {
                int sumDays = 0;
                for (int i = dateTimeInt1[0]; i < dateTimeInt2[0]; i++)
                {
                    sumDays += getYearDays(i);
                }
                return sumDays + dateTime2.DayOfYear - dateTime1.DayOfYear;
            }
        }


        //计算一年天数
        static int getYearDays(int year)
        {
            if (DateTime.IsLeapYear(year))
            {
                return 366;
            }
            else return 365;
        }


        private void Search_Click(object sender, RoutedEventArgs e)
        {
            LoadText(@"..\..\..\search\" + ConstellationOutput.Text + ".txt");
        }

        public void LoadText(string ad)
        {


            FileStream fs;
            if (File.Exists(ad))
            {
                fs = new FileStream(ad, FileMode.Open, FileAccess.Read);
                using (fs)
                {
                    TextRange text = new TextRange(Rbox.Document.ContentStart, Rbox.Document.ContentEnd);
                    text.Load(fs, DataFormats.Text);
                }

            }
        }



        public MainWindow()
        {
            InitializeComponent();
        }


    }
}
