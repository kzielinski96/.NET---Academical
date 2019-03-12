using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Lab01
{
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public BitmapImage PersonImage { get; set; }
    }
}
