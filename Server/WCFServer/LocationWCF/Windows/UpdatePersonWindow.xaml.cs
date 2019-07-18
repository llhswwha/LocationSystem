using BLL;
using DbModel.Location.AreaAndDev;
using DbModel.Location.Person;
using DbModel.Location.Relation;
using Location.BLL.Tool;
using LocationServices.Locations.Services;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace LocationServer.Windows
{
    /// <summary>
    /// UpdatePersonWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UpdatePersonWindow : Window
    {
        public UpdatePersonWindow()
        {
            InitializeComponent();
        }

        DataTable tb;

        private Department AddDepartment(string name,Department parent)
        {
            Department dep = new Department(name, parent.Id);
            var r1 = db.Departments.Add(dep);
            if (r1 == false)
            {
                MessageBox.Show("添加部门失败:" + name + "\n" + db.Departments.ErrorMessage);
                return null;
            }
            departments.Add(dep);
            Log.Info("添加部门:"+ name);
            return dep;
        }

        Bll db;
        List<Department> departments;

        private void MenuUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tb == null)
                {
                    MessageBox.Show("表格数据不存在");
                    return;
                }

                db = Bll.NewBllNoRelation();//第三个参数要true，不然数据迁移无法用
                departments = db.Departments.ToList();

                var parentDepName = "3DL粉焦区域";//"中电四会热电有限公司"
                var parentDep = departments.Find(i => i.Name.Trim() == parentDepName);
                if (parentDep == null)
                {
                    parentDep = AddDepartment(parentDepName, departments[1]);
                }
                if (parentDep == null) return;

                var ps = new PersonService(db);
                foreach (DataRow item in tb.Rows)
                {
                    try
                    {
                        var departmentName = (item[0] + "").Trim();
                        var personName = (item[1] + "").Trim();
                        var personCount = (item[2] + "").Trim();
                        var personNumber = (item[3] + "").Trim();
                        var cardCode = (item[4] + "").Trim();
                        var post = (item[5] + "").Trim();
                        var remark = (item[6] + "").Trim();

                        //添加部门
                        var department = departments.Find(i => i.Name.Trim() == departmentName);
                        if (department == null)
                        {
                            department = AddDepartment(departmentName, parentDep);
                        }
                        if (parentDep == null) return;

                        LocationCard card = null;
                        if (!string.IsNullOrEmpty(cardCode))
                        {
                            card = db.LocationCards.Find(i => i.Code.Trim() == cardCode);
                            if (card == null)
                            {
                                //MessageBox.Show("未找到定位卡:" + cardCode + "\n请确认数据是否正确！");//必须有正确的卡号
                                //return;

                                card = new LocationCard(cardCode, 2);//管理人员卡
                                var r1 = db.LocationCards.Add(card);
                                if (r1 == false)
                                {
                                    MessageBox.Show("添加定位卡失败:" + cardCode + "\n" + db.LocationCards.ErrorMessage);
                                    return;
                                }
                                else
                                {
                                    Log.Info("添加定位卡:" + cardCode);
                                }
                            }
                            else
                            {

                            }
                        }

                        if(personName== "刘钢")
                        {

                        }

                        Personnel person = db.Personnels.Find(i => i.Name.Trim() == personName);
                        var list = db.LocationCardToPersonnels.ToList();
                        if (list != null && card!=null)
                        {
                            var cTpList = list.Where(i => i.LocationCardId == card.Id).ToList();
                            if (cTpList != null && cTpList.Count > 0)//已经存在关系
                            {
                                if (cTpList.Count == 1)//只有一个关系
                                {
                                    var cTp0 = cTpList[0];

                                    Personnel person2 = db.Personnels.Find(i => i.Id == cTp0.PersonnelId);
                                    if (person2 != null)
                                    {
                                        if (person2.Name.StartsWith("Tag_"))//自动创建的
                                        {
                                            EditPerson(person2, department, personName, personNumber, post);//更新人员信息
                                            return;
                                        }
                                        else
                                        {

                                        }
                                    }
                                    else
                                    {

                                    }

                                }
                                else//多个关系
                                {
                                    foreach (LocationCardToPersonnel cpt in cTpList)
                                    {
                                        if (person != null)
                                        {
                                            if (cpt.PersonnelId == person.Id)
                                                continue;
                                        }
                                        db.LocationCardToPersonnels.Remove(cpt);//清空以前的卡关联关系
                                    }
                                }
                            }
                            else
                            {

                            }
                        }
 

                        //Personnel person = db.Personnels.Find(i => i.Name.Trim() == personName);
                        if (person == null)
                        {
                            person = AddPerson(department, personName, personNumber, post);
                            if (person == null) return;
                        }
                        else
                        {
                            EditPerson(person, department, personName, personNumber, post);
                        }

                        if (card != null)
                        {
                            var cTp1 = list.Find(i => i.LocationCardId == card.Id);
                            var cTp2 = list.Find(i => i.PersonnelId == person.Id);

                            var r2 = ps.BindWithTag(person.Id, card.Id);

                            var cTpList2 = db.LocationCardToPersonnels.Where(i => i.LocationCardId == card.Id);

                            if (r2 == false)
                            {
                                //MessageBox.Show("设置绑定失败:" + personName + "," + cardCode + "\n" + db.Personnels.ErrorMessage);
                                continue;
                            }
                            else
                            {
                                Log.Info("设置绑定:" + personName + "," + cardCode);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("解析并写入数据出错:" + ex);
                        return;
                    }
                }

                MessageBox.Show("更新成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("更新出错:" + ex);
            }
        }

        private Personnel AddPerson(Department department, string personName, string personNumber, string post)
        {
            Personnel person = new Personnel(personName, department, personNumber);
            person.Pst = post;
            var r1 = db.Personnels.Add(person);
            if (r1 == false)
            {
                MessageBox.Show("添加人员失败:" + personName + "\n" + db.Personnels.ErrorMessage);
                return null;
            }
            else
            {
                Log.Info("添加人员:" + personName);
            }
            return person;
        }

        private void EditPerson(Personnel person,Department department,string personName,string personNumber,string post)
        {
            person.Name = personName;
            person.ParentId = department.Id;
            person.WorkNumber = personNumber;
            person.Pst = post;


            var r1 = db.Personnels.Edit(person);
            if (r1 == false)
            {
                MessageBox.Show("修改人员失败:" + personName + "\n" + db.Personnels.ErrorMessage);
                return;
            }
            else
            {
                Log.Info("修改人员:" + personName);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\定位终端人员卡号.xlsx";
            if (!File.Exists(path))
            {
                MessageBox.Show("文件不存在:"+path);
                return;
            }
            try
            {
                tb = ExcelLib.ExcelHelper.LoadTable(new FileInfo(path), null, true);
                DataGrid1.ItemsSource = tb.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取文件失败:" + path+"\n"+ex);
            }
            
        }
    }
}
