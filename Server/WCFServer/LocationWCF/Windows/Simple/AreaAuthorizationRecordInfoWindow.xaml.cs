using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using BLL;
using DbModel.Location.Authorizations;
using DbModel.Location.Work;
using Location.TModel.Location.AreaAndDev;
using LocationServices.Locations.Services;

namespace LocationServer.Windows
{
    /// <summary>
    /// Interaction logic for AreaAuthorizationRecordInfoWindow.xaml
    /// </summary>
    public partial class AreaAuthorizationRecordInfoWindow : Window
    {
        public AreaAuthorizationRecordInfoWindow()
        {
            InitializeComponent();
        }

        private void CbAreaList_OnKeyUp(object sender, KeyEventArgs e)
        {
            //List<string> mylist = new List<string>();
            var mylist = areaList.Where(i => i.Name.Contains(CbAreaList.Text));
            CbAreaList.ItemsSource = mylist;
            CbAreaList.IsDropDownOpen = true;
        }

        private IList<PhysicalTopology> areaList; 

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TbName.Text = "权限New";
            TbDescription.Text = "权限New...";
            CbAccessList.ItemsSource = Enum.GetValues(typeof (AreaAccessType));
            CbAccessList.SelectedIndex = 0;
            CbRangeTypeList.ItemsSource = Enum.GetValues(typeof (AreaRangeType));
            CbRangeTypeList.SelectedIndex = 0;
            CbTimeType.ItemsSource = Enum.GetValues(typeof (RepeatDay));
            CbTimeType.SelectedIndex = 0;
            LoadAreaList();
            LoadRoleList();

            StartTime.Value = new DateTime(2000, 1, 1, 8, 30, 0);
            EndTime.Value = new DateTime(2000, 1, 1, 17, 30, 0);
        }

        private void LoadRoleList()
        {
            var service = new TagRoleService();
            CbRoleList.ItemsSource = service.GetList();
            CbRoleList.SelectedIndex = 0;
        }

        private void LoadAreaList()
        {
            var service = new AreaService();
            areaList = service.GetList();
            CbAreaList.ItemsSource = areaList;
            CbAreaList.SelectedIndex = 0;
        }

        private void BtnSubmit_OnClick(object sender, RoutedEventArgs e)
        {
            Bll bll = AppContext.GetLocationBll();
            var aa = new AreaAuthorization();
            aa.Name = TbName.Text;
            aa.Description = TbDescription.Text;

            var area = CbAreaList.SelectedItem as PhysicalTopology;
            aa.AreaId = area.Id;

            if (bll.AreaAuthorizations.Add(aa) == false)
            {
                MessageBox.Show("添加失败1");
            }

            var role = CbAreaList.SelectedItem as CardRole;
            
            var aar = new AreaAuthorizationRecord(aa,role);

            if (bll.AreaAuthorizationRecords.Add(aar)==false)
            {
                MessageBox.Show("添加失败2");
            }

            MessageBox.Show("添加成功");
        }
    }
}
