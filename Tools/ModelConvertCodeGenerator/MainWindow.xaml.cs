using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using CommunicationClass.SihuiThermalPowerPlant.Models;
using DbModel.Location.AreaAndDev;
using Location.TModel.ConvertCodes;
using Location.TModel.Location.AreaAndDev;

namespace ModelConvertCodeGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnGenerateOne_OnClick(object sender, RoutedEventArgs e)
        {
            Type type1 = AssemblyTypeInfo1.CurrentType;
            Type type2 = AssemblyTypeInfo2.CurrentType;
            ConvertCodeGenerator coder = new ConvertCodeGenerator(type1, type2);
            TbCode.Text = coder.GetCode();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            Assembly assembly1 = typeof (Tag).Assembly;
            Assembly assembly2 = typeof (LocationCard).Assembly;
            //Assembly assembly2 = typeof(sis).Assembly;
            
            //Assembly assembly2 = typeof(tickets).Assembly;
            AssemblyTypeInfo1.Assembly = assembly1;
            AssemblyTypeInfo2.Assembly = assembly2;
        }

        private void AssemblyTypeInfo1_OnSelectedTypeChanged(object arg1, Type arg2)
        {
            Type type = AssemblyTypeInfo2.FindSimilarType(arg2);
            AssemblyTypeInfo2.SetCurrentType(type);
        }

        private void BtnGenerateAll_OnClick(object sender, RoutedEventArgs e)
        {
            List<Type> types1 = AssemblyTypeInfo1.Types;
            List<Type> types2 = AssemblyTypeInfo2.Types;
            TbCode.Text = ConvertCodeGenerator.GetCode(types1, types2);
        }
    }
}
