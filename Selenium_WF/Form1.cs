using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;

namespace Selenium_WF
{
    public partial class Form1 : Form
    {
        static IWebDriver driverChrome;
        IWebElement parent;
        List<string> JobTitlesList;
        string[] Aplhabets;
        string RealtimeLogs;

        public Form1()
        {
            InitializeComponent();
            RealtimeLogs = "Application Launched \n";
            rt_Logs.Text += RealtimeLogs.ToString();
            var chromeDriverService = ChromeDriverService.CreateDefaultService(@"C:\chromedriver_win32");
            chromeDriverService.HideCommandPromptWindow = true;
            driverChrome =  new ChromeDriver(chromeDriverService, new ChromeOptions());
           // driverChrome = new ChromeDriver(@"C:\chromedriver_win32");
            
            RealtimeLogs = "Initialized Web Driver for Chrome \n";
            rt_Logs.Text += RealtimeLogs.ToString();
            RealtimeLogs = "Processing.... \n";
            rt_Logs.Text += RealtimeLogs.ToString();
            JobTitlesList = new List<string>();
            Aplhabets = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            //Aplhabets = new string[] { "C" };
        }

        private void btn_Click_Click(object sender, EventArgs e)
        {

            driverChrome.Navigate().GoToUrl("http://www.careerbuilder.com/");
            BrowserToJobs();
            RealtimeLogs = "Exporting Listings to Excel File... \n";
            rt_Logs.Text += RealtimeLogs.ToString();
            ExportDatatoExcel();
            RealtimeLogs = "Listings Exported Successfully. \n";
            rt_Logs.Text += RealtimeLogs.ToString();
            RealtimeLogs = "Please view 'Listings.csv'\n";
            rt_Logs.Text += RealtimeLogs.ToString();
        }

        public void BrowserToJobs()
        {
            driverChrome.FindElement(By.Id("header-menu-browse-jobs-link")).SendKeys(OpenQA.Selenium.Keys.Enter);

            for (int i = 0; i < Aplhabets.Length; i++)
            {
                JobListingByAlphabet(Aplhabets[i]);
            }
        }

        public void JobListingByAlphabet(string Alphabet)
        {
            parent = driverChrome.FindElement(By.ClassName("browse-job-titles"));
            parent.FindElement(By.XPath(".//a[contains(text(),'" + Alphabet + "')]")).Click();

            GetListings();
        }

        public void GetListings()
        {
            var listoftitles = driverChrome.FindElements(By.ClassName("browse-titles-link"));
            foreach (var title in listoftitles)
            {
                var temp_anchor = title.FindElement(By.TagName("a"));
                JobTitlesList.Add(temp_anchor.Text);
            }
        }

        public void ExportDatatoExcel()
        {
            if (File.Exists("listings.csv"))
            {
                File.Delete("listings.csv");
            }
            using (StreamWriter sr = File.CreateText("listings.csv"))
            {
                foreach (string listitem in JobTitlesList)
                {
                    sr.WriteLine(listitem);
                }
            }
        }
    }
}
