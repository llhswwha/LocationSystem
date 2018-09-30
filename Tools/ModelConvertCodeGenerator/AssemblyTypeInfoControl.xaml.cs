using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Location.TModel.ConvertCodes;

namespace ModelConvertCodeGenerator
{
    /// <summary>
    /// Interaction logic for TypeInfoControl.xaml
    /// </summary>
    public partial class AssemblyTypeInfoControl : UserControl
    {
        private Assembly _assembly;

        public AssemblyTypeInfoControl()
        {
            InitializeComponent();
        }

        public List<Type> Types { get; set; }

        public Assembly Assembly
        {
            get { return _assembly; }
            set
            {
                _assembly = value;
                if (comboBox == null) return;

                Types = new List<Type>();
                foreach (Type i in _assembly.GetTypes())
                {
                    if (i.BaseType == typeof (Attribute)) continue;

                    if (i.IsClass
                        && !i.IsAbstract
                        && !i.FullName.Contains("Tools")
                        && !i.FullName.Contains("InitInfo")
                        && !i.FullName.Contains("ConvertCodes"))
                    {
                        ObsoleteAttribute obsoleteAttribute=i.GetCustomAttribute<ObsoleteAttribute>();
                        if (obsoleteAttribute != null)
                        {
                            continue;
                        }
                        Types.Add(i);
                    }
                }
                textBlock.Text = value.ToString() + "\n" + Types.Count;
                comboBox.ItemsSource = Types;
            }
        }

        public Type CurrentType { get; set; }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentType = comboBox.SelectedItem as Type;
            if (CurrentType == null) return;
            PropertyInfo[] propertyInfos = CurrentType.GetProperties();
            listBox.ItemsSource = propertyInfos;
            OnSelectedTypeChanged(CurrentType);
        }

        public event Action<object,Type> SelectedTypeChanged;

        protected void OnSelectedTypeChanged(Type type)
        {
            if (SelectedTypeChanged != null)
            {
                SelectedTypeChanged(this, type);
            }
        }

        public Type FindSimilarType(Type type1)
        {
            return ConvertCodeGenerator.FindSimilarType(Types,type1);
        }

        public void SetCurrentType(Type type)
        {
            comboBox.SelectedItem = type;
        }
    }
}
