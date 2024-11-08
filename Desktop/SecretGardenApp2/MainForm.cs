using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;
using System.Linq;
using System.Data.SqlClient;
using System.Net;
using SG_Server_Interface;
using SG_Server_Interface.Classes;
using SG_Server_Interface.Responses.UserRouteResponses.Login;
using SG_Server_Interface.Responses.ChildResponsetRoutes.GetChild;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using SG_Server_Interface.Responses.EmailsResponseRoute.GetEmails;
using System.Reflection.PortableExecutable;
using SG_Server_Interface.Responses.EmailsResponseRoute.SendEmail;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using SG_Server_Interface.Responses.UserResponseRoutes.SearchAll;
using SG_Server_Interface.Responses.ChildResponsetRoutes.GetAllChildren;
using SG_Server_Interface.Responses.UserResponseRoutes.AddUser;
using SG_Server_Interface.Responses.ChildResponsetRoutes.AddChild;
using SG_Server_Interface.Responses.Resources.GetResourcesMedia;
using SG_Server_Interface.Responses.Gallery.GetAllIImages;
using SG_Server_Interface.Responses.Gallery.GetEvents;
using SG_Server_Interface.Responses.UserResponseRoutes.SearchSpecific;
using Org.BouncyCastle.Utilities;
using System.Net.Http;
using SG_Server_Interface.Responses.UserResponseRoutes.GetParentContact;
using SG_Server_Interface.Responses.Calendar.GetCalendar;
using SG_Server_Interface.Responses.Resources.AddResouce;
using SG_Server_Interface.Responses.Gallery.AddEvents;
using SG_Server_Interface.Responses.Gallery.GetYears;
using SG_Server_Interface.Responses.Newsletters.GetNewsletter;
using SG_Server_Interface.Responses.Calendar.AddCalendar;
using SG_Server_Interface.Responses.Newsletters.AddNewsletter;
using SG_Server_Interface.Responses.UserResponseRoutes.ResetPassword;
using SG_Server_Interface.Responses.UserResponseRoutes.ForgotPassword;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;


#pragma warning disable CA1416

namespace SecretGardenApp2
{
    public partial class MainForm : Form
    {
        private readonly string API_URL = "http://192.168.6.239:3000";

        private List<Emails> emailList = new List<Emails>();
        private List<Email> emailList2 = new List<Email>();
        private List<Email> emailList3 = new List<Email>();

        private APIInterface api_int;
        private User user;
        private List<Child> children;
        private List<Emails> emails;
        public MainForm()
        {
            InitializeComponent();
            this.Load += new EventHandler(MainForm_Load); // Ensure the Load event is bound

            TbcMain.Appearance = TabAppearance.FlatButtons;
            TbcMain.ItemSize = new System.Drawing.Size(0, 1);
            TbcMain.SizeMode = TabSizeMode.Fixed;

            this.api_int = new();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            textBox17.PasswordChar = '*';
            textBox18.PasswordChar = '*';
        }

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = TBUsername.Text.ToLower();
            string password = TBPassword.Text;

            LoginResponse loginResponse = await api_int.UserRoute.Login(username, password);

            int statusCode = loginResponse.StatusCode;
            if (statusCode == 200)
            {
                user = loginResponse.User;
                if (user.is_admin == 0)
                {
                    TbcMain.SelectedTab = Inbox;
                    pictureBox9.ImageLocation = $"{this.API_URL}{user.image_path}";
                    textBox1.Text = user.first_name;
                    textBox2.Text = user.last_name;
                    label20.Text = "Username:";
                    textBox3.Text = user.username;
                    textBox5.Text = user.contact_no;
                    LoadAdminsIntoComboBox();
                }
                else
                {
                    TbcMain.SelectedTab = AdminInbox;
                    pictureBox20.ImageLocation = $"{this.API_URL}{user.image_path}";
                    textBox11.Text = user.first_name;
                    textBox10.Text = user.last_name;
                    label41.Text = "Username:";
                    textBox9.Text = user.username;
                    textBox7.Text = user.contact_no;
                    LoadUsersIntoComboBox();
                }
                LoadEmails();
                LoadEmails2();
                LoadArtsResources();
                LoadAdditionalResources();
                LoadMediaResources();
                UpdateTipLabels();
                LoadUpcomingEvents();
                await LoadAndDisplayEvents();
            }
            else if (statusCode == 404)
            {
                MessageBox.Show(this, "The username or password you entered is incorrect. Please try again.", "Authentication Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show(this, "An internal server error occurred. Please try again later.", "Server Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button21_MouseEnter(object sender, EventArgs e)
        {
            TBPassword.PasswordChar = '\0';
        }

        private void button21_MouseLeave(object sender, EventArgs e)
        {
            TBPassword.PasswordChar = '*';
        }

        //USER VIEWING THEIR KIDS USING THE COMBOBOX
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected name
            string selectedItem = comboBox3.SelectedItem.ToString();

            int index = comboBox3.SelectedIndex;

            if (index == 0)
            {
                pictureBox9.ImageLocation = $"{this.API_URL}{user.image_path}";
                textBox1.Text = user.first_name;
                textBox2.Text = user.last_name;
                label20.Text = "Username:";
                textBox3.Text = user.username;
                textBox5.Text = user.contact_no;
            }
            else
            {
                Child c = this.children[index - 1];
                pictureBox9.ImageLocation = $"{this.API_URL}{c.image_path}";
                textBox1.Text = c.first_name;
                textBox2.Text = c.last_name;
                label20.Text = "Class:";
                textBox3.Text = c.class_name;
                textBox5.Text = this.user.contact_no;
            }
        }

        private async void LoadEmails()
        {
            // Update the query to sort by timestamp in descending order

            GetEmailsResponse getEmailsResponse = await api_int.EmailRoute.GetEmailsByUsername(user.username);

            if (getEmailsResponse.Code == 404)
            {
                listBox1.Items.Clear();
                MessageBox.Show("No emails available.", "No Emails", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (getEmailsResponse.Code == 500)
            {
                listBox1.Items.Clear();
                //MessageBox.Show("No emails available.", "No Emails", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                this.emails = getEmailsResponse.Emails;
                listBox1.Items.Clear();
                foreach (Emails email in this.emails)
                {
                    listBox1.Items.Add($"From: {email.sender_username} - Subject: {email.subject}");
                }

            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = listBox1.SelectedIndex;

            // Check if selectedIndex is valid
            if (selectedIndex >= 0 && selectedIndex < emails.Count)
            {
                Emails selectedEmail = emails[selectedIndex];

                label21.Text = $"From: {selectedEmail.sender_username}\n" +
                               $"Body: {selectedEmail.body}\n" +
                               $"Sent: {selectedEmail.timestamp}";

                label4.Text = $"Subject: {selectedEmail.subject}\n";
            }
            else if (selectedIndex == -1)
            {
                // Handle the case when no valid item is selected
                label21.Text = string.Empty;
                label4.Text = "Please select an email";
            }
        }



        //ADMIN VIEWING MESSAGE REQUESTS
        private async void LoadEmails2()
        {

            GetEmailsResponse getEmailsResponse = await api_int.EmailRoute.GetEmailsByUsername(user.username);

            if (getEmailsResponse.Code == 404)
            {
                listBox2.Items.Clear();
                MessageBox.Show("No emails were found.", "No Emails Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (getEmailsResponse.Code == 500)
            {
                listBox2.Items.Clear();
                //MessageBox.Show("No emails were found.", "No Emails Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                this.emails = getEmailsResponse.Emails;
                listBox2.Items.Clear();
                foreach (Emails email in this.emails)
                {
                    listBox2.Items.Add($"From: {email.sender_username} - Subject: {email.subject}");
                }

            }


        }

        //ADMIN VIEWING A MESSAGE REQUEST AND MOVING IT TO OPENED
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Ensure an item is selected
            if (listBox2.SelectedIndex == -1)
                return;

            int selectedIndex = listBox2.SelectedIndex;

            // Ensure the selected index is within the bounds of the emails list
            if (selectedIndex >= 0 && selectedIndex < emails.Count)
            {
                Emails selectedEmail = emails[selectedIndex];

                label124.Text = $"Subject: {selectedEmail.subject}";
                label105.Text = $"From: {selectedEmail.sender_username}\n" +
                                $"Body: {selectedEmail.body}\n" +
                                $"Sent: {selectedEmail.timestamp}";
            }
            else
            {
                // Optionally handle cases where the selected index is out of bounds
                label124.Text = "Subject:";
                label105.Text = "From:\nBody:\nSent:";
            }
        }


        //ADMIN VIEWING OPENED EMAILS
        private void button1_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminOpened;
            LoadEmails3();
        }

        private async void LoadEmails3()
        {

            GetEmailsResponse getEmailsResponse = await api_int.EmailRoute.GetEmailsByUsername(user.username);

            if (getEmailsResponse.Code == 404)
            {
                listBox3.Items.Clear();
                MessageBox.Show("No emails were found.", "No Emails Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (getEmailsResponse.Code == 500)
            {
                listBox3.Items.Clear();
                //MessageBox.Show("No emails were found.", "No Emails Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                this.emails = getEmailsResponse.Emails;
                listBox3.Items.Clear();
                foreach (Emails email in this.emails)
                {
                    if (email.status == "read")
                    {
                        listBox3.Items.Add($"From: {email.sender_username} - Subject: {email.subject}");
                    }
                }

            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

            int selectedIndex = listBox3.SelectedIndex;

            Emails selectedEmail = emails[selectedIndex];

            MessageBox.Show($"From: {selectedEmail.sender_username}\n" +
                            $"Subject: {selectedEmail.subject}\n" +
                            $"Body: {selectedEmail.body}\n" +
                            $"Sent: {selectedEmail.timestamp}");
        }

        //ADMIN VIEW ALL KIDS ON THE USER SEARCH TAB
        private void AddKidPictureBox(string picturePath, string kidName)
        {
            PictureBox pictureBox = CreatePictureBox(picturePath, kidName);
            flowLayoutPanelKids.Controls.Add(pictureBox);
        }

        private PictureBox CreatePictureBox(string picturePath, string kidName)
        {
            PictureBox pictureBox = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                Width = 150,
                Height = 150,
                Tag = kidName
            };

            // Try loading the image from the provided URL
            try
            {
                pictureBox.ImageLocation = !string.IsNullOrEmpty(picturePath) ? picturePath : "default-image-path.jpg";
            }
            catch
            {
                // In case of any issue loading the image, load a default image
                pictureBox.ImageLocation = "default-image-path.jpg";
            }

            pictureBox.Click += PictureBox_Click;
            return pictureBox;
        }

        private Panel CreateKidPanel(string picturePath, string kidName)
        {
            Panel kidPanel = new Panel
            {
                Width = 150,
                Height = 200,
                Padding = new Padding(5)
            };

            kidPanel.Controls.Add(CreatePictureBox(picturePath, kidName));
            kidPanel.Controls.Add(CreateKidLabel(kidName));

            return kidPanel;
        }

        private Label CreateKidLabel(string kidName)
        {
            Label label = new Label
            {
                Text = kidName,
                Font = new Font("Century Gothic", 11), // Set the font to Century Gothic with size 12
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Bottom,
                Height = 30
            };
            return label;
        }


        private List<Child> allChildren;

        private async void LoadAllKids()
        {
            flowLayoutPanelKids.Controls.Clear();

            try
            {
                GetAllChildrenResponse res = await this.api_int.ChildRoute.GetAllChildren(user.username);
                allChildren = res.Children;
                foreach (Child c in res.Children)
                {
                    flowLayoutPanelKids.Controls.Add(CreateKidPanel($"{API_URL}{(c.image_path.StartsWith('/') ? c.image_path : (" / " + c.image_path))}", $"{c.first_name} {c.last_name}"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading kids. Please try again later.\nError details: " + ex.Message,
                "Loading Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
        }


        //ADMIN SEARCHING FOR A SPECIFIC KID USING THE TEXTBOX
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = textBox4.Text.ToLower();
            UpdateKidsDisplay(searchTerm);
        }

        // Method to update the display based on the search term
        private void UpdateKidsDisplay(string searchTerm)
        {
            flowLayoutPanelKids.Controls.Clear();

            try
            {
                var filteredChildren = allChildren.Where(c =>
                    c.first_name.ToLower().Contains(searchTerm) ||
                    c.last_name.ToLower().Contains(searchTerm)).ToList();

                foreach (Child c in filteredChildren)
                {
                    flowLayoutPanelKids.Controls.Add(CreateKidPanel($"{API_URL}{c.image_path}", $"{c.first_name} {c.last_name}"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating the display. Please try again later.\nError details: " + ex.Message,
                "Update Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            }
        }

        //ADMIN VIEWING A CHILD AFTER PRESSING ON THEIR IMAGE IN USER SEARCH
        private void PictureBox_Click(object sender, EventArgs e)
        {
            PictureBox clickedPictureBox = sender as PictureBox;
            string kidName = clickedPictureBox.Tag.ToString();

            TbcMain.SelectedTab = AdminChildViewer;
            LoadKidProfile(kidName);

        }

        // Method to load the kid profile
        private void LoadKidProfile(string kidName)
        {

            try
            {
                var kid = allChildren.FirstOrDefault(c => $"{c.first_name} {c.last_name}" == kidName);

                if (kid != null)
                {
                    GetChildId(kid.id);
                    textBox25.Text = kid.first_name;
                    textBox26.Text = kid.last_name;
                    textBox27.Text = kid.class_name; // Assuming class name is mapped to 'class_name'
                    string fp = "";
                    if (kid.image_path.StartsWith('/'))
                    {
                        fp = $"{API_URL}{kid.image_path}";
                    }
                    else
                    {
                        fp = $"{API_URL}/{kid.image_path}";
                    }
                    //textBox28.Text = kid.contactno;
                    pictureBox28.ImageLocation = fp;
                    label125.Text = kid.notes;
                }
                else
                {
                    MessageBox.Show("Kid's profile not found. Please ensure the name is correct.", "Profile Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading the profile. Please try again later.\nError details: " + ex.Message,
                                "Loading Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        List<ParentContact> contactdetails;

        public async void GetChildId(int child_id)
        {
            try
            {
                // Call the API to get the parent contact information
                ParentContactResponse res = await api_int.UserRoute.GetParentContact(child_id);

                contactdetails = res.Parents;

                cmbParent.Items.Clear();
                cmbParent.Text = "select parent";

                if (res != null && res.Parents.Any())
                {
                    // Process the parent contact information here
                    foreach (var parent in res.Parents)
                    {
                        // You can display or use the parent contact info as needed
                        string parentInfo = $"{parent.first_name} {parent.last_name}";
                        cmbParent.Items.Add(parentInfo);
                    }
                }
                else
                {
                    MessageBox.Show("No parent contact information found.",
                    "Contact Information Not Found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while retrieving the information. Please try again later.\nError details: {ex.Message}",
                     "Information Retrieval Error",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
            }
        }

        private void cmbParent_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = cmbParent.SelectedIndex;
            if (index == -1)
            {
                textBox28.Text = "No parent selected";
            }
            else
            {
                textBox28.Text = contactdetails[index].contact_no;
            }
        }


        private void label26_Click(object sender, EventArgs e)
        {
            textBox4.Clear();
        }

        //LOGOUT BUTTON FOR USER
        private void button2_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = LoginPage;
            TBUsername.Clear();
            TBPassword.Clear();
            comboBox3.Items.Clear();
        }

        //LOGOUT BUTTON FOR ADMIN
        private void button9_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = LoginPage;
            TBUsername.Clear();
            TBPassword.Clear();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = UserMedia;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = UserArtsandCrafts;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = UserTips;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = UserResources;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = UserSend;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = UserRefer;
        }

        private void textBox20_Click(object sender, EventArgs e)
        {
            textBox20.Clear();
        }

        private void textBox19_Click(object sender, EventArgs e)
        {
            textBox19.Clear();
        }

        private void textBox21_Click(object sender, EventArgs e)
        {
            textBox21.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = UserSecurity;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = UserAddChild;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminCompose;
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminChildViewer;
        }

        private void pictureBox16_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminViewResources;
        }

        private async void pictureBox19_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminCurrentGallery;
            GetYearResponse res = await api_int.GalleryRoute.GetYears();
            if (res.Code == 500)
            {
                MessageBox.Show("Internal server error");
            }
            else
            {
                comboBox2.Items.Clear();
                res.Years.ForEach(year => comboBox2.Items.Add(year));
            }
        }

        private void pictureBox23_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminViewCalendar;
        }

        private void textBox30_Click(object sender, EventArgs e)
        {
            textBox30.Clear();
        }

        private void textBox8_Click(object sender, EventArgs e)
        {
            textBox8.Clear();
        }

        private void pictureBox17_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminAddResource;
            textBox8.Visible = false;
            label40.Visible = false;
        }

        private void pictureBox18_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminAddGallery;
        }

        private void pictureBox22_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminAddCalendar;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminSecurity;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminAddUser;
        }

        //ADMIN SENDING MESSAGE
        private async void button7_Click(object sender, EventArgs e)
        {
            string receiver = comboBox4.Text;
            string receiverUsername = receiver.Split(' ')[0];
            string subject = textBox23.Text;
            string body = textBox22.Text;
            string sender1 = TBUsername.Text;

            SendEmailResponse res = await this.api_int.EmailRoute.SendEmail(sender1, receiver, subject, body);

            if (res.Code == 200)
            {
                MessageBox.Show("Message sent successfully!", "Email has been sent", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (res.Code == 404)
            {
                MessageBox.Show("recipient not found");
            }
            else
            {
                MessageBox.Show("Please enter a valid username");
            }

            textBox23.Text = "Subject:";
            comboBox4.Text = "To:";
            textBox22.Text = "Message:";
        }

        private void button175_Click(object sender, EventArgs e)
        {
            textBox23.Text = "Subject:";
            comboBox4.Text = "To:";
            textBox22.Text = "Message:";
        }

        private async void LoadUsersIntoComboBox()
        {

            SearchAllUsernamesResponse res = await this.api_int.UserRoute.GetAllUsernames(user.username);

            comboBox4.Items.Clear();

            foreach (string username in res.Usernames)
            {
                comboBox4.Items.Add(username);
            }

        }

        //USER REQUESTING TO SEND MESSAGE
        private async void button16_Click(object sender, EventArgs e)
        {

            string receiver = comboBox5.Text;
            string receiverUsername = receiver.Split(' ')[0];
            string subject = textBox20.Text;
            string body = textBox19.Text;
            string sender1 = TBUsername.Text;

            SendEmailResponse res = await this.api_int.EmailRoute.SendEmail(sender1, receiver, subject, body);

            if (res.Code == 200)
            {
                MessageBox.Show("Message sent successfully!", "Email has been sent", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (res.Code == 404)
            {
                MessageBox.Show("Recipient not found");
            }
            else
            {
                MessageBox.Show("Please enter a Valid Username");
            }


            textBox20.Text = "Subject:";
            comboBox5.Text = "To:";
            textBox19.Text = "Message:";

        }

        private void button173_Click(object sender, EventArgs e)
        {
            textBox20.Text = "Subject:";
            comboBox5.Text = "To:";
            textBox19.Text = "Message:";
        }

        private async void LoadAdminsIntoComboBox()
        {
            SearchAllUsernamesResponse res = await this.api_int.UserRoute.GetAllUsernames(user.username);

            comboBox5.Items.Clear();

            foreach (string username in res.Usernames)
            {
                comboBox5.Items.Add(username);
            }
        }

        //USER SENDING A REFERAL
        private void button15_Click(object sender, EventArgs e)
        {

            textBox21.Text = "Refer to:";
            MessageBox.Show("Referal Sent!");
        }

        private void pictureBox65_Click(object sender, EventArgs e)
        {
            string facebookUrl = "https://www.facebook.com/SecretGardenNurserySchool/";
            Process.Start(facebookUrl);
        }

        private void pictureBox66_Click(object sender, EventArgs e)
        {
            string webUrl = "https://secretgarden-nurseryschool.co.za/";
            Process.Start(webUrl);
        }


        private void textBox22_Click(object sender, EventArgs e)
        {
            textBox22.Clear();
        }

        private void textBox23_Click(object sender, EventArgs e)
        {
            textBox23.Clear();
        }

        //USER VIWING THE ADDITIONAL RESOURCES
        private async void LoadAdditionalResources()
        {
            GetFileResouceResponse res = await api_int.ResourceRoute.GetFileResource("additional");

            List<FileResource> files = res.Resources;

            listBox10.Items.Clear();
            listBox17.Items.Clear();

            foreach (FileResource file in files)
            {
                string[] ext = file.file_path.Split('.');
                listBox10.Items.Add(new MediaResource { Id = $"{file.id}", Title = $"{file.title} ({ext[ext.Length - 1].ToUpper()})", FilePath = $"{API_URL}{file.file_path}" });
                listBox17.Items.Add(new MediaResource { Id = $"{file.id}", Title = $"{file.title} ({ext[ext.Length - 1].ToUpper()})", FilePath = $"{API_URL}{file.file_path}" });
            }
        }

        private async void listBox10_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox10.SelectedItem is MediaResource selectedResource)
            {
                string fileExtension = Path.GetExtension(selectedResource.FilePath).ToLower();

                // Define image file extensions
                string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif" }; // Add more as needed
                string[] pdfExtensions = { ".pdf" }; // Define PDF file extensions

                // Check if the file extension matches any of the image types
                if (imageExtensions.Contains(fileExtension))
                {
                    try
                    {
                        // Download the image from the URL
                        using (HttpClient httpClient = new HttpClient())
                        {
                            var imageBytes = await httpClient.GetByteArrayAsync(selectedResource.FilePath);
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                pictureBox6.Image = Image.FromStream(ms);
                                pictureBox6.SizeMode = PictureBoxSizeMode.Zoom; // Adjust size mode if necessary
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred while loading the image: " + ex.Message);
                    }
                }
                // Check if the file extension matches any of the PDF types
                else if (pdfExtensions.Contains(fileExtension))
                {
                    try
                    {
                        // Open the PDF in the default web browser
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = selectedResource.FilePath, // This should be the full URL to the PDF
                            UseShellExecute = true // Opens the PDF in the default browser
                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred while opening the PDF: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("The selected media resource is neither an image nor a PDF.");
                }
            }
            else
            {
                MessageBox.Show("Please select a media resource to proceed.", "No Media Resource Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void listBox17SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox17.SelectedItem is MediaResource selectedResource)
            {
                string fileExtension = Path.GetExtension(selectedResource.FilePath).ToLower();

                // Define image file extensions
                string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif" }; // Add more as needed
                string[] pdfExtensions = { ".pdf" }; // Define PDF file extensions

                // Check if the file extension matches any of the image types
                if (imageExtensions.Contains(fileExtension))
                {
                    try
                    {
                        // Download the image from the URL
                        using (HttpClient httpClient = new HttpClient())
                        {
                            var imageBytes = await httpClient.GetByteArrayAsync(selectedResource.FilePath);
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                pictureBox106.Image = Image.FromStream(ms);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while loading the image. Please try again.\nDetails: {ex.Message}", "Image Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                // Check if the file extension matches any of the PDF types
                else if (pdfExtensions.Contains(fileExtension))
                {
                    try
                    {
                        // Open the PDF in the default web browser
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = selectedResource.FilePath, // This should be the full URL to the PDF
                            UseShellExecute = true // Opens the PDF in the default browser
                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred while opening the PDF: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("The selected media resource is neither an image nor a PDF.");
                }
            }
            else
            {
                MessageBox.Show("Please select a media resource to proceed.", "No Media Resource Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //USER VIEWING THE UPLOADED ARTS RESOURCES
        private async void LoadArtsResources()
        {
            GetFileResouceResponse res = await api_int.ResourceRoute.GetFileResource("arts_and_crafts");

            List<FileResource> files = res.Resources;

            listBox5.Items.Clear();
            listBox7.Items.Clear();
            listBox14.Items.Clear();
            listBox15.Items.Clear();

            foreach (FileResource file in files)
            {
                if (IsVideoFile(file.file_path))
                {
                    listBox5.Items.Add(new MediaResource { Id = $"{file.id}", Title = file.title, FilePath = $"{API_URL}{file.file_path}" });
                    listBox15.Items.Add(new MediaResource { Id = $"{file.id}", Title = file.title, FilePath = $"{API_URL}{file.file_path}" });
                }
                else
                {
                    listBox7.Items.Add(new MediaResource { Id = $"{file.id}", Title = file.title, FilePath = $"{API_URL}{file.file_path}" });
                    listBox14.Items.Add(new MediaResource { Id = $"{file.id}", Title = file.title, FilePath = $"{API_URL}{file.file_path}" });
                }
            }
        }

        //USER PLAYING THE ARTS RESOURCES TUTORIAL VIDEOS
        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check if an item is selected in the list box
            if (listBox5.SelectedItem is MediaResource selectedResource)
            {
                string fileExtension = Path.GetExtension(selectedResource.FilePath).ToLower();

                // Define video file extensions
                string[] videoExtensions = { ".mp4", ".avi", ".mov", ".wmv" };

                // Check if the file extension matches any of the video types
                if (videoExtensions.Contains(fileExtension))
                {
                    // Display message indicating which video is being played
                    MessageBox.Show($"Now playing video: {selectedResource.Title}", "Video Playback", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Set the active tab to the video player tab
                    TbcMain.SelectedTab = VideoPlayer;

                    // Set the URL for the media player and start playback
                    axWindowsMediaPlayer1.URL = selectedResource.FilePath;

                    // Optional: Check if the media is ready before playing
                    //axWindowsMediaPlayer1.PlayStateChange += AxWindowsMediaPlayer1_PlayStateChange;
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                }
                else
                {
                    MessageBox.Show("The selected media resource is not a video.");
                }
            }
            else
            {
                MessageBox.Show("Please select a media resource to proceed.", "No Media Resource Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Event handler to handle play state changes
        private void AxWindowsMediaPlayer1_PlayStateChange(int newState)
        {
            // Check if the video has finished playing
            if (newState == (int)WMPLib.WMPPlayState.wmppsMediaEnded)
            {
                MessageBox.Show("Video playback has ended.", "Playback Finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //ADMIN PLAYING ARTS AND C
        private void listBox15_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check if an item is selected in the list box
            if (listBox15.SelectedItem is MediaResource selectedResource)
            {
                string fileExtension = Path.GetExtension(selectedResource.FilePath).ToLower();

                // Define video file extensions
                string[] videoExtensions = { ".mp4", ".avi", ".mov", ".wmv" };

                // Check if the file extension matches any of the video types
                if (videoExtensions.Contains(fileExtension))
                {
                    // Display message indicating which video is being played
                    MessageBox.Show($"Now playing: {selectedResource.Title}", "Video Playback", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Set the active tab to the video player tab
                    TbcMain.SelectedTab = AdminVideoPlayer;

                    // Set the URL for the media player and start playback
                    axWindowsMediaPlayer2.URL = selectedResource.FilePath;

                    // Optional: Check if the media is ready before playing
                    //axWindowsMediaPlayer1.PlayStateChange += AxWindowsMediaPlayer1_PlayStateChange;
                    axWindowsMediaPlayer2.Ctlcontrols.play();
                }
                else
                {
                    MessageBox.Show("The selected media resource is not a video.");
                }
            }
            else
            {
                MessageBox.Show("Please select a media resource to proceed.", "No Media Resource Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        //USER VIEWING THE DRAWINGS FROM ARTS RESOURCES
        private void listBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check if an item is selected in the list box
            if (listBox7.SelectedItem is MediaResource selectedResource)
            {
                try
                {
                    // Open the image URL in the default web browser
                    System.Diagnostics.Process.Start(new ProcessStartInfo
                    {
                        FileName = selectedResource.FilePath,
                        UseShellExecute = true // This opens the URL in the default web browser
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while opening the resource: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select a media resource to proceed.", "No Media Resource Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //ADMIN ARTS AND C
        private void listBo14_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check if an item is selected in the list box
            if (listBox14.SelectedItem is MediaResource selectedResource)
            {
                try
                {
                    // Open the image URL in the default web browser
                    System.Diagnostics.Process.Start(new ProcessStartInfo
                    {
                        FileName = selectedResource.FilePath,
                        UseShellExecute = true // This opens the URL in the default web browser
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while opening the resource: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select a media resource to proceed.", "No Media Resource Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tbcmain_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Assuming tabPageVideoPlayer is the tab page containing the video player
            if (TbcMain.SelectedTab != VideoPlayer)
            {
                // Stop the media player if it's playing
                if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
                {
                    axWindowsMediaPlayer1.Ctlcontrols.stop();
                }
            }
            if (TbcMain.SelectedTab != AdminVideoPlayer)
            {
                // Stop the media player if it's playing
                if (axWindowsMediaPlayer2.playState == WMPLib.WMPPlayState.wmppsPlaying)
                {
                    axWindowsMediaPlayer2.Ctlcontrols.stop();
                }
            }
        }




        //USER DOWNLOADING THE ARTS RESOURCES
        private void button22_Click(object sender, EventArgs e)
        {
            if (pictureBox12.Image == null)
            {
                MessageBox.Show("There is no image available to download.", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png;*.gif;*.bmp)|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            saveFileDialog.Title = "Save Image As";

            // Check if the user selected a file
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Save the image to the selected path
                    pictureBox12.Image.Save(saveFileDialog.FileName);
                    MessageBox.Show("The image has been downloaded successfully.", "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while saving the image. Please try again.\nDetails: {ex.Message}", "Image Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //USER VIEWING THE UPLOADED MEDIA RESOURCES
        private async void LoadMediaResources()
        {
            GetFileResouceResponse res = await api_int.ResourceRoute.GetFileResource("media");

            List<FileResource> files = res.Resources;

            listBox4.Items.Clear();
            listBox6.Items.Clear();
            listBox13.Items.Clear();
            listBox12.Items.Clear();

            foreach (FileResource file in files)
            {
                if (file.file_path.EndsWith(".mp4"))
                {
                    listBox4.Items.Add(new MediaResource { Id = $"{file.id}", Title = file.title, FilePath = $"{API_URL}{file.file_path}" });
                    listBox12.Items.Add(new MediaResource { Id = $"{file.id}", Title = file.title, FilePath = $"{API_URL}{file.file_path}" });
                }
                else
                {
                    listBox6.Items.Add(new MediaResource { Id = $"{file.id}", Title = file.title, FilePath = $"{API_URL}{file.file_path}" });
                    listBox13.Items.Add(new MediaResource { Id = $"{file.id}", Title = file.title, FilePath = $"{API_URL}{file.file_path}" });
                }
            }
        }


        private bool IsVideoFile(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            return extension == ".mp4" || extension == ".avi" || extension == ".mov" || extension == ".wmv" || extension == ".flv"; // Add other video extensions as needed
        }

        private bool IsAudioFile(string extension)
        {
            string[] audioExtensions = { ".mp3", ".wav", ".ogg", ".aac" };
            return audioExtensions.Contains(extension);
        }

        private bool IsImageFile(string extension)
        {
            string[] audioExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            return audioExtensions.Contains(extension);
        }

        //USER PLAYING THE AUDIO FROM MEDIA RESOURCES
        private void listBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox6.SelectedItem is MediaResource selectedResource)
            {
                // Determine the file extension
                string fileExtension = Path.GetExtension(selectedResource.FilePath).ToLower();

                // Define video file extensions
                string[] videoExtensions = { ".mp3", ".wav", ".ogg", ".aac" };

                // Check if the file extension matches any of the video types
                if (videoExtensions.Contains(fileExtension))
                {
                    MessageBox.Show($"Now playing audio: {selectedResource.Title}", "Playing Audio", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TbcMain.SelectedTab = VideoPlayer;
                    axWindowsMediaPlayer1.URL = $"{selectedResource.FilePath}";
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                }
                else
                {
                    MessageBox.Show("The selected media resource is not a video.", "Invalid Media Type", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("No media resource selected. Please select a resource to continue.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //USER PLAYING THE VIDEO FROM MEDIA RESOURCES
        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox4.SelectedItem is MediaResource selectedResource)
            {
                // Determine the file extension
                string fileExtension = Path.GetExtension(selectedResource.FilePath).ToLower();

                // Define video file extensions
                string[] videoExtensions = { ".mp4", ".avi", ".mov", ".wmv" };

                // Check if the file extension matches any of the video types
                if (videoExtensions.Contains(fileExtension))
                {
                    MessageBox.Show($"Playing video: {selectedResource.Title}", "Video Playback", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TbcMain.SelectedTab = VideoPlayer;
                    axWindowsMediaPlayer1.URL = $"{selectedResource.FilePath}";
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                }
                else
                {
                    MessageBox.Show("The selected media resource is not a video.", "Invalid Media Type", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("No media resource selected. Please select a resource to continue.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        //ADMIN PLAYING MEDIA RESOURCES
        private void listBox13_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox13.SelectedItem is MediaResource selectedResource)
            {
                // Determine the file extension
                string fileExtension = Path.GetExtension(selectedResource.FilePath).ToLower();

                // Define audio file extensions
                string[] audioExtensions = { ".mp3", ".wav", ".ogg", ".aac" };

                // Check if the file extension matches any of the audio types
                if (audioExtensions.Contains(fileExtension))
                {
                    MessageBox.Show($"Playing audio: {selectedResource.Title}", "Audio Playback", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TbcMain.SelectedTab = AdminVideoPlayer;
                    axWindowsMediaPlayer2.URL = $"{selectedResource.FilePath}";
                    axWindowsMediaPlayer2.Ctlcontrols.play();
                }
                else
                {
                    MessageBox.Show("The selected media resource is not an audio file.", "Invalid Media Type", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("No media resource selected. Please select a resource to continue.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void listBox12_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox12.SelectedItem is MediaResource selectedResource)
            {
                // Determine the file extension
                string fileExtension = Path.GetExtension(selectedResource.FilePath).ToLower();

                // Define video file extensions
                string[] videoExtensions = { ".mp4", ".avi", ".mov", ".wmv" };

                // Check if the file extension matches any of the video types
                if (videoExtensions.Contains(fileExtension))
                {
                    MessageBox.Show($"Playing video: {selectedResource.Title}", "Video Playback", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TbcMain.SelectedTab = AdminVideoPlayer;
                    axWindowsMediaPlayer2.URL = $"{selectedResource.FilePath}";
                    axWindowsMediaPlayer2.Ctlcontrols.play();
                }
                else
                {
                    MessageBox.Show("The selected media resource is not a video.", "Invalid Media Type", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("No media resource selected. Please select a resource to continue.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        //ADMIN ADDING RESOURCES
        private async void button12_Click(object sender, EventArgs e)
        {

            string type = string.Empty;
            string title = textBox30.Text;
            string uploadedBy = TBUsername.Text;
            string selectedResourceType = comboBox1.SelectedItem.ToString();

            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Please provide a title.", "Missing Title", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tipText = textBox8.Text;
            string filePath = "";

            switch (selectedResourceType)
            {
                case "Media":
                    type = "media";

                    try
                    {
                        filePath = textBox6.Text; // Get the file path from the text box
                        string[] _ = File.ReadAllLines(filePath);
                    }
                    catch (FileNotFoundException)
                    {
                        MessageBox.Show(this, "No files have been inserted!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    AddResourceResponse mres = await api_int.ResourceRoute.AddResource(title, type, user.username, filePath, true);
                    if (mres.Code == 400)
                    {
                        MessageBox.Show(this, mres.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (mres.Code == 500)
                    {
                        MessageBox.Show(this, "An error occurred on our end. Please try again later or contact support if the issue persists.", "Server Issue", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Resource added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;

                case "Arts and Crafts":
                    type = "arts_and_crafts";

                    try
                    {
                        filePath = textBox6.Text; // Get the file path from the text box
                        string[] _ = File.ReadAllLines(filePath);
                    }
                    catch (FileNotFoundException)
                    {
                        MessageBox.Show(this, "No files have been inserted!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    AddResourceResponse acres = await api_int.ResourceRoute.AddResource(title, type, user.username, filePath, true);
                    if (acres.Code == 400)
                    {
                        MessageBox.Show(this, acres.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (acres.Code == 500)
                    {
                        MessageBox.Show(this, "An error occurred on our end. Please try again later or contact support if the issue persists.", "Server Issue", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Resource added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;

                case "Additional Resources":
                    type = "additional";

                    try
                    {
                        filePath = textBox6.Text; // Get the file path from the text box
                        string[] _ = File.ReadAllLines(filePath);
                    }
                    catch (FileNotFoundException)
                    {
                        MessageBox.Show(this, "No files have been inserted!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    AddResourceResponse ares = await api_int.ResourceRoute.AddResource(title, type, user.username, filePath, true);
                    if (ares.Code == 400)
                    {
                        MessageBox.Show(this, ares.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (ares.Code == 500)
                    {
                        MessageBox.Show(this, "An error occurred on our end. Please try again later or contact support if the issue persists.", "Server Issue", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Resource added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    break;

                case "Tips":
                    type = "tips";

                    if (tipText == string.Empty)
                    {
                        MessageBox.Show("no tip added");
                        return;
                    }

                    AddResourceResponse tres = await api_int.ResourceRoute.AddResource(title, type, user.username, tipText, false);
                    if (tres.Code == 400)
                    {
                        MessageBox.Show(this, tres.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (tres.Code == 500)
                    {
                        MessageBox.Show(this, tres.Message, "internal server error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Resource added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    break;

                default:
                    MessageBox.Show("Invalid resource type selected.");
                    return;
            }

            ClearFields();
        }


        private string GetTableName(string resourceType)
        {
            switch (resourceType)
            {
                case "Media":
                    return "media_resources";
                case "Arts and Crafts":
                    return "arts_and_crafts_resources";
                case "Additional Resources":
                    return "additional_resources";
                default:
                    return "";
            }
        }

        private void ClearFields()
        {
            textBox30.Text = "Add resource";
            textBox6.Text = "file path";
            textBox8.Text = "Description";
        }

        private void panel57_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0 && IsSupportedFile(files[0]))
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void panel57_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0 && IsSupportedFile(files[0]))
                {
                    textBox6.Text = files[0];
                }
            }
        }

        private bool IsSupportedFile(string filePath)
        {
            string[] videoExtensions = { ".mp4", ".avi", ".mov", ".wmv" };
            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".pdf" };
            string[] audioExtensions = { ".mp3", ".wav", ".aac", ".flac", ".ogg" };
            string extension = Path.GetExtension(filePath).ToLower();

            return videoExtensions.Contains(extension) || imageExtensions.Contains(extension) || audioExtensions.Contains(extension);
        }

        //WHEN THE ADMIN SELECTS TIPS ON THE ADD RESOURCES PAGE
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedResourceType = comboBox1.SelectedItem.ToString();

            if (selectedResourceType == "Tips")
            {
                // Show the large text box for tips
                textBox8.Visible = true;
                label40.Visible = true;


                // Hide the drag-and-drop panel and file path text box
                panel57.Visible = false;
                textBox6.Visible = false;
                label29.Visible = false;
            }
            else
            {
                // Hide the large text box for tips
                textBox8.Visible = false;
                label40.Visible = false;

                // Show the drag-and-drop panel and file path text box
                panel57.Visible = true;
                textBox6.Visible = true;
                label29.Visible = true;
            }
        }

        //USER VIEWING TIPS
        List<TipResouce> tips_resouces;
        private async void UpdateTipLabels()
        {
            // Fetch the tip resources from the API
            GetTipResouceResponse res = await api_int.ResourceRoute.GetTipResource();

            // Store the resources in the tips_resouces list
            tips_resouces = res.Resouces;

            // Clear the list boxes before adding new items to avoid duplication
            listBox9.Items.Clear();
            listBox16.Items.Clear();

            // Add the titles of the tips to the list boxes
            foreach (TipResouce tip in tips_resouces)
            {
                listBox9.Items.Add(tip.title);
                listBox16.Items.Add(tip.title);
            }
        }


        private void listBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listBox9.SelectedIndex;

            label66.Text = tips_resouces[index].title;
            label116.Text = tips_resouces[index].tip_text;
        }
        private void listBox16_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listBox16.SelectedIndex;

            label99.Text = tips_resouces[index].title;
            label96.Text = tips_resouces[index].tip_text;
        }

        //ADMIN ADDING PICTURES TO GALLERY
        private async void button18_Click(object sender, EventArgs e)
        {
            // Assuming textBox12 contains the folder path
            string folderPath = textBox12.Text;
            string eventName = textBox31.Text; // TextBox for event name
            string year = textBox13.Text; // TextBox for the year

            if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
            {
                MessageBox.Show("Please provide a valid folder path to continue.", "Invalid Folder Path", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get all image files from the folder
            string[] imageFiles = Directory.GetFiles(folderPath, "*.*").Where(file =>
                file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                file.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                file.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)).ToArray();

            // Check if there are any image files to upload
            if (imageFiles.Length == 0)
            {
                MessageBox.Show("No images were found in the selected folder. Please select a folder containing images to continue.", "No Images Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (string imagePath in imageFiles)
            {
                string fileName = Path.GetFileName(imagePath);

                // Call the API with the file path and other details
                AddEventResponse res = await api_int.GalleryRoute.AddEvent(eventName, year, imagePath); // Pass the image path directly

                if (res.Code == 400)
                {
                    MessageBox.Show(this, res.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (res.Code == 500)
                {
                    MessageBox.Show(this, "An internal error occurred. Please try again later or contact support if the issue continues.", "Server Issue", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    MessageBox.Show("Images added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            textBox12.Text = "file path";
            textBox13.Clear();
            textBox31.Text = "Add events to gallery";
        }



        private void panel39_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                // Check if the dropped item is a folder
                if (files.Length > 0 && Directory.Exists(files[0]))
                {
                    e.Effect = DragDropEffects.Copy; // Allow drop
                }
                else
                {
                    e.Effect = DragDropEffects.None; // Disallow drop
                }
            }
            else
            {
                e.Effect = DragDropEffects.None; // Disallow drop
            }
        }

        private void panel39_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (files.Length > 0)
                {
                    // Check if the dropped item is a folder
                    if (Directory.Exists(files[0]))
                    {
                        string folderPath = files[0];
                        textBox12.Text = folderPath;
                    }
                    else
                    {
                        MessageBox.Show("Please drop a folder to continue.", "Folder Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }


        //USER VIEWING GALLERY
        // Event handler for when the year is selected in the ComboBox
        private async void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {

            pictureBox144.Visible = false;
            // Clear existing controls
            flowLayoutPanel1.Controls.Clear();

            string selectedYear = cmbYear.SelectedItem.ToString();

            try
            {
                // Get event names for the selected year
                GetEventNamesResponse eventNamesResponse = await api_int.GalleryRoute.GetEventNames(selectedYear);

                if (eventNamesResponse?.EventNames == null)
                {
                    throw new Exception("Event names response is null or contains no event names.");
                }

                HashSet<string> eventsDisplayed = new HashSet<string>();

                // Loop through each event name and get images for that event
                for (int i = 0; i < eventNamesResponse.EventNames.Count; i++)
                {
                    // Loop through each image and add it to the FlowLayoutPanel
                    string imagePath;
                    if (!eventNamesResponse.ImagePaths[i].StartsWith('/'))
                    {
                        imagePath = $"{API_URL}/{eventNamesResponse.ImagePaths[i]}";
                    }
                    else
                    {
                        imagePath = $"{API_URL}{eventNamesResponse.ImagePaths[i]}";
                    }


                    // Create a container Panel for each image and its label
                    Panel imagePanel = new Panel
                    {
                        Width = 150, // Adjust the width as needed
                        Height = 170, // Adjust the height to accommodate both image and label
                        Padding = new Padding(0, 0, 0, 5) // Add bottom padding to space out label
                    };

                    // Create and configure PictureBox to load images from server URL
                    PictureBox pb = new PictureBox
                    {
                        ImageLocation = imagePath, // Load image from server URL
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Width = 130, // Set appropriate size
                        Height = 130, // Set appropriate size
                        Dock = DockStyle.Top,
                        Tag = new { ImagePath = imagePath, EventName = eventNamesResponse.EventNames[i] } // Store image path and event name in Tag property
                    };
                    pb.Click += PictureBox1_Click; // Attach the click event handler

                    // Create and configure Label
                    Label lbl = new Label
                    {
                        Text = eventNamesResponse.EventNames[i],
                        Font = new Font("Century Gothic", 11), // Set the font to Century Gothic
                        TextAlign = ContentAlignment.MiddleCenter,
                        Dock = DockStyle.Bottom,
                        Height = 30, // Set appropriate height
                        Margin = new Padding(0) // Remove any extra margin
                    };

                    // Add PictureBox and Label to the Panel
                    imagePanel.Controls.Add(pb);
                    imagePanel.Controls.Add(lbl);

                    // Add Panel to FlowLayoutPanel
                    flowLayoutPanel1.Controls.Add(imagePanel);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading images: " + ex.Message, "Image Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox145.Visible = false;
            // Clear existing controls
            flowLayoutPanel2.Controls.Clear();

            string selectedYear = comboBox2.SelectedItem.ToString();

            try
            {
                // Get event names for the selected year
                GetEventNamesResponse eventNamesResponse = await api_int.GalleryRoute.GetEventNames(selectedYear);

                if (eventNamesResponse?.EventNames == null)
                {
                    throw new Exception("Event names response is null or contains no event names.");
                }

                HashSet<string> eventsDisplayed = new HashSet<string>();

                // Loop through each event name and get images for that event
                for (int i = 0; i < eventNamesResponse.EventNames.Count; i++)
                {
                    // Loop through each image and add it to the FlowLayoutPanel
                    string imagePath;
                    if (!eventNamesResponse.ImagePaths[i].StartsWith('/'))
                    {
                        imagePath = $"{API_URL}/{eventNamesResponse.ImagePaths[i]}";
                    }
                    else
                    {
                        imagePath = $"{API_URL}{eventNamesResponse.ImagePaths[i]}";
                    }


                    // Create a container Panel for each image and its label
                    Panel imagePanel = new Panel
                    {
                        Width = 150, // Adjust the width as needed
                        Height = 170, // Adjust the height to accommodate both image and label
                        Padding = new Padding(0, 0, 0, 5) // Add bottom padding to space out label
                    };

                    // Create and configure PictureBox to load images from server URL
                    PictureBox pb = new PictureBox
                    {
                        ImageLocation = imagePath, // Load image from server URL
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Width = 130, // Set appropriate size
                        Height = 130, // Set appropriate size
                        Dock = DockStyle.Top,
                        Tag = new { ImagePath = imagePath, EventName = eventNamesResponse.EventNames[i] } // Store image path and event name in Tag property
                    };
                    pb.Click += PictureBoxx1_Click; // Attach the click event handler

                    // Create and configure Label
                    Label lbl = new Label
                    {
                        Text = eventNamesResponse.EventNames[i],
                        Font = new Font("Century Gothic", 11), // Set the font to Century Gothic
                        TextAlign = ContentAlignment.MiddleCenter,
                        Dock = DockStyle.Bottom,
                        Height = 30, // Set appropriate height
                        Margin = new Padding(0) // Remove any extra margin
                    };

                    // Add PictureBox and Label to the Panel
                    imagePanel.Controls.Add(pb);
                    imagePanel.Controls.Add(lbl);

                    // Add Panel to FlowLayoutPanel
                    flowLayoutPanel2.Controls.Add(imagePanel);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading images: " + ex.Message, "Image Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        // Event handler for when a PictureBox is clicked
        private void PictureBox1_Click(object sender, EventArgs e)
        {
            PictureBox clickedPictureBox = sender as PictureBox;
            var tagData = (dynamic)clickedPictureBox.Tag;
            string imagePath = tagData.ImagePath;
            string eventName = tagData.EventName;

            // Switch to the userGallery tab
            TbcMain.SelectedTab = UserGallery;

            // Load gallery images based on the clicked image's event
            LoadGalleryImages(eventName, cmbYear.Text);
        }

        private void PictureBoxx1_Click(object sender, EventArgs e)
        {
            PictureBox clickedPictureBox = sender as PictureBox;
            var tagData = (dynamic)clickedPictureBox.Tag;
            string imagePath = tagData.ImagePath;
            string eventName = tagData.EventName;

            // Switch to the userGallery tab
            TbcMain.SelectedTab = AdminViewGallery;

            // Load gallery images based on the clicked image's event
            LoadGalleryImages1(eventName, comboBox2.Text);
        }

        private List<string> eventImagePaths = new List<string>();
        private int currentImageIndex = 0;

        // Method to load gallery images based on the clicked event
        private async void LoadGalleryImages(string eventName, string year)
        {
            eventImagePaths.Clear(); // Clear previous images
            currentImageIndex = 0; // Reset index

            GetAllImagesResponse res = await api_int.GalleryRoute.GetAllImages(year, eventName);



            foreach (GalleryImage image in res.Images)
            {
                string imagePath;
                if (!image.image_path.StartsWith('/'))
                {
                    imagePath = $"{API_URL}/{image.image_path}";
                }
                else
                {
                    imagePath = $"{API_URL}{image.image_path}";
                }

                eventImagePaths.Add(imagePath); // Add image paths to the list
            }
            DisplayCurrentImage();
        }

        // Display the current image in pictureBox25
        private void DisplayCurrentImage()
        {
            if (eventImagePaths.Count > 0 && currentImageIndex >= 0 && currentImageIndex < eventImagePaths.Count)
            {
                // Load image from URL instead of local file (if applicable)
                pictureBox25.ImageLocation = eventImagePaths[currentImageIndex];
            }
        }

        private List<string> eventImagePaths1 = new List<string>();
        private int currentImageIndex1 = 0;

        // Method to load gallery images based on the clicked event
        private async void LoadGalleryImages1(string eventName, string year)
        {
            eventImagePaths1.Clear(); // Clear previous images
            currentImageIndex1 = 0; // Reset index

            GetAllImagesResponse res = await api_int.GalleryRoute.GetAllImages(year, eventName);

            foreach (GalleryImage image in res.Images)
            {
                string imagePath;
                if (!image.image_path.StartsWith('/'))
                {
                    imagePath = $"{API_URL}/{image.image_path}";
                }
                else
                {
                    imagePath = $"{API_URL}{image.image_path}";
                }

                eventImagePaths1.Add(imagePath); // Add image paths to the list
            }
            DisplayCurrentImage1();
        }

        private void DisplayCurrentImage1()
        {
            if (eventImagePaths1.Count > 0 && currentImageIndex1 >= 0 && currentImageIndex1 < eventImagePaths1.Count)
            {
                // Load image from URL instead of local file (if applicable)
                pictureBox122.ImageLocation = eventImagePaths1[currentImageIndex1];
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (eventImagePaths.Count > 0)
            {
                // Move to the next image, looping back to the start
                currentImageIndex = (currentImageIndex + 1) % eventImagePaths.Count;
                DisplayCurrentImage();
            }
        }

        private void button172_Click(object sender, EventArgs e)
        {
            if (eventImagePaths1.Count > 0)
            {
                // Move to the next image, looping back to the start
                currentImageIndex1 = (currentImageIndex1 + 1) % eventImagePaths1.Count;
                DisplayCurrentImage1();
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string destinationFolder = folderBrowserDialog.SelectedPath;

                    // Get the selected image URL based on the selected index
                    int selectedIndex = currentImageIndex; // Assuming you have a ListBox or similar control
                    if (selectedIndex < 0 || selectedIndex >= eventImagePaths.Count)
                    {
                        MessageBox.Show("Please select an image to download.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string imageUrl = eventImagePaths[selectedIndex].Replace(" ", "%20"); // Get the URL of the selected image

                    try
                    {
                        // Get the filename from the URL
                        string fileName = Path.GetFileName(imageUrl);
                        string destinationPath = Path.Combine(destinationFolder, fileName);

                        // Check if the URL is well-formed
                        if (Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
                        {
                            using (WebClient webClient = new WebClient())
                            {
                                webClient.DownloadFile(imageUrl, destinationPath); // Download the image
                            }

                            MessageBox.Show("Image downloaded successfully.", "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("The URL is not valid: " + imageUrl, "Invalid URL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred while downloading the image: " + ex.Message, "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void button171_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string destinationFolder = folderBrowserDialog.SelectedPath;

                    // Get the selected image URL based on the selected index
                    int selectedIndex1 = currentImageIndex1; // Assuming you have a ListBox or similar control
                    if (selectedIndex1 < 0 || selectedIndex1 >= eventImagePaths1.Count)
                    {
                        MessageBox.Show("Please select an image to download.", "Image Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string imageUrl = eventImagePaths1[selectedIndex1].Replace(" ", "%20"); // Get the URL of the selected image

                    try
                    {
                        // Get the filename from the URL
                        string fileName = Path.GetFileName(imageUrl);
                        string destinationPath = Path.Combine(destinationFolder, fileName);

                        // Check if the URL is well-formed
                        if (Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
                        {
                            using (WebClient webClient = new WebClient())
                            {
                                webClient.DownloadFile(imageUrl, destinationPath); // Download the image
                            }

                            MessageBox.Show("Image downloaded successfully.", "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("The URL is not valid: " + imageUrl, "Invalid URL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred while downloading the image: " + ex.Message, "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }



        //ADMIN ADDING TO CALENDAR
        private async void btnAddEvent_Click(object sender, EventArgs e)
        {
            string title = textBox32.Text;
            string description = textBox14.Text;
            DateTime eventDate = dateTimePicker2.Value.AddDays(1);

            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Title is required.");
                return;
            }

            // Format the date as required by your API, typically in "yyyy-MM-dd" format
            string formattedEventDate = eventDate.ToString("yyyy-MM-dd");

            // Pass the formattedEventDate as a string to the API
            AddCalendarResponse res = await api_int.CallendarRoute.AddCalenderEvent(title, description, formattedEventDate);

            if (res.Code == 400)
            {
                MessageBox.Show(this, res.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (res.Code == 500)
            {
                MessageBox.Show(this, res.Message, "Internal Server Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                MessageBox.Show("Calendar event added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            textBox32.Text = "Add event to Calendar";
            textBox14.Clear();
        }


        //VIEWING CALENDAR
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime selectedDate = dateTimePicker1.Value.Date; // Ensure only the date part is considered
            textBox16.Clear();
            textBox15.Clear();
            UpdateListBox8(selectedDate);
            textBox16.Text = "Please Select a Date";
        }

        //admin calendar
        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            DateTime selectedDate1 = dateTimePicker3.Value.Date; // Ensure only the date part is considered
            textBox16.Clear();
            textBox15.Clear();
            UpdateListBox18(selectedDate1);
            textBox52.Text = "Please Select a Date";
        }


        List<CalendarEvent> activeEvents = new List<CalendarEvent>();
        private async void UpdateListBox8(DateTime selectedDate)
        {
            GetCalendarEventResponse res = await api_int.CallendarRoute.GetCalendarEvents();

            List<CalendarEvent> calendarEvents = res.Events;

            listBox8.Items.Clear();
            activeEvents.Clear(); // Clear the list to avoid accumulating events

            foreach (CalendarEvent calendarEvent in calendarEvents)
            {
                if (selectedDate.Date == calendarEvent.event_date.Date)
                {
                    listBox8.Items.Add(calendarEvent.title);
                    activeEvents.Add(calendarEvent);
                }
            }
            DisplayUpcomingEvents(calendarEvents);
        }

        private async Task LoadAndDisplayEvents()
        {
            GetCalendarEventResponse res = await api_int.CallendarRoute.GetCalendarEvents();
            List<CalendarEvent> calendarEvents = res.Events;

            // Initially populate listBox8 and activeEvents for the selected date
            DateTime selectedDate = dateTimePicker1.Value.Date;
            listBox8.Items.Clear();
            activeEvents.Clear();
            foreach (CalendarEvent calendarEvent in calendarEvents)
            {
                if (selectedDate.Date == calendarEvent.event_date.Date)
                {
                    listBox8.Items.Add(calendarEvent.title);
                    activeEvents.Add(calendarEvent);
                }
            }

            // Display all upcoming events
            DisplayUpcomingEvents(calendarEvents);
        }

        private void DisplayUpcomingEvents(List<CalendarEvent> calendarEvents)
        {
            // Sort events by event_date in ascending order
            var sortedEvents = calendarEvents.OrderBy(e => e.event_date).ToList();

            // Create a formatted string of all upcoming events
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Upcoming Events:");

            foreach (var calendarEvent in sortedEvents)
            {
                sb.AppendLine($"{calendarEvent.event_date:MM/dd/yyyy} - {calendarEvent.title}");
            }

            // Display the formatted string in textBox15
            textBox15.Text = sb.ToString();
            textBox53.Text = sb.ToString();
        }



        //admin calendar
        List<CalendarEvent> activeEvents11 = new List<CalendarEvent>();
        private async void UpdateListBox18(DateTime selectedDate1)
        {
            GetCalendarEventResponse res = await api_int.CallendarRoute.GetCalendarEvents();

            List<CalendarEvent> calendarEvents1 = res.Events;

            listBox18.Items.Clear();
            activeEvents11.Clear(); // Clear the list to avoid accumulating events

            foreach (CalendarEvent calendarEvent in calendarEvents1)
            {
                if (selectedDate1.Date == calendarEvent.event_date.Date)
                {
                    listBox18.Items.Add(calendarEvent.title);
                    activeEvents11.Add(calendarEvent); // Corrected to add to activeEvents11
                }
            }
            DisplayUpcomingEvents(calendarEvents1);
        }


        private void listBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox8.SelectedIndex != -1) // Ensure an item is selected
            {
                CalendarEvent @event = activeEvents[listBox8.SelectedIndex];
                textBox16.Text = @event.title;
                textBox15.Text = @event.description;
            }
        }


        //admin calendar
        private void listBox18_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox18.SelectedIndex != -1) // Ensure an item is selected
            {
                CalendarEvent @event = activeEvents11[listBox18.SelectedIndex];
                textBox52.Text = @event.title;
                textBox53.Text = @event.description;
            }
        }



        //USER VIEWING NEWSLETTER

        List<Newsletter> activeEvents1 = new List<Newsletter>();
        private async void LoadUpcomingEvents()
        {
            GetNewslettersResponse res = await api_int.NewsletterRoute.GetNewsletters();

            listBox11.Items.Clear();
            listBox19.Items.Clear();
            activeEvents1.Clear();

            List<Newsletter> newsletters = res.newsletters;

            foreach (Newsletter newsletter in newsletters)
            {

                listBox11.Items.Add(newsletter.title);
                listBox19.Items.Add(newsletter.title);
                activeEvents1.Add(newsletter);

            }

        }

        private void listBox11_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox11.SelectedItem != null)
            {
                Newsletter @event = activeEvents1[listBox11.SelectedIndex];
                textBox44.Text = @event.title;
                textBox45.Text = @event.description;
                pictureBox7.ImageLocation = $"{this.API_URL}{@event.image_path}";
            }
        }

        private void listBox19_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox19.SelectedItem != null)
            {
                Newsletter @event = activeEvents1[listBox19.SelectedIndex];
                textBox54.Text = @event.title;
                textBox55.Text = @event.description;
                pictureBox142.ImageLocation = $"{this.API_URL}{@event.image_path}";
            }
        }

        //USER CHANGING PASSWORD
        private void button17_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = UserChangePassword;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminChangePassword;
        }

        //USER CHANGING PASSWORD
        private async void button6_Click(object sender, EventArgs e)
        {
            string username = TBUsername.Text; // Assuming you have a textbox for username
            string currentPassword = textBox17.Text; // Assuming you have a textbox for current password
            string newPassword = textBox18.Text; // Assuming you have a textbox for new password

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Please ensure all fields are filled out before proceeding.", "Incomplete Form", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            ResetPasswordResponse res = await api_int.UserRoute.ResetPassword(username, currentPassword, newPassword);

            if (res.Code == 404)
            {
                MessageBox.Show(this, res.Message, "Resource Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (res.Code == 403)
            {
                MessageBox.Show(this, "Access denied. Please check your permissions.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (res.Code == 500)
            {
                MessageBox.Show("An internal server error occurred. Please try again later or contact support.", "Server Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                MessageBox.Show("Changes have been successfully applied.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            textBox17.Clear();
            textBox18.Clear();
        }

        //ADMIN CHANGING PASSWORD
        private async void button113_Click(object sender, EventArgs e)
        {
            string username = TBUsername.Text; // Assuming you have a textbox for username
            string currentPassword = textBox49.Text; // Assuming you have a textbox for current password
            string newPassword = textBox24.Text; // Assuming you have a textbox for new password

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Please ensure all fields are filled out before proceeding.", "Incomplete Form", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            ResetPasswordResponse res = await api_int.UserRoute.ResetPassword(username, currentPassword, newPassword);

            if (res.Code == 404)
            {
                MessageBox.Show(this, res.Message, "Resource Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (res.Code == 403)
            {
                MessageBox.Show(this, "Access denied. Please check your permissions.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (res.Code == 500)
            {
                MessageBox.Show("An internal server error occurred. Please try again later or contact support.", "Server Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                MessageBox.Show("Changes have been successfully applied.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            textBox49.Clear();
            textBox24.Clear();
        }


        private void panel45_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Get the list of dropped files
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Check if the first file is supported
                if (files.Length > 0 && IsSupportedFile1(files[0]))
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void panel45_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Get the list of dropped files
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Check if the first file is supported and update the text box
                if (files.Length > 0 && IsSupportedFile1(files[0]))
                {
                    textBox37.Text = files[0];
                }
                else
                {
                    MessageBox.Show("Unsupported file type.");
                }
            }
        }

        private bool IsSupportedFile1(string filePath)
        {
            string[] supportedExtensions = { ".mp4", ".avi", ".mov", ".wmv",  // Video formats
                                     ".jpg", ".jpeg", ".png", ".gif", ".bmp", // Image formats
                                     ".mp3", ".wav", ".aac", ".flac", ".ogg" }; // Audio formats

            string extension = Path.GetExtension(filePath).ToLower();

            return supportedExtensions.Contains(extension);
        }

        private void button25_Click(object sender, EventArgs e)
        {
            InsertUser();
            textBox29.Clear();
            textBox33.Clear();
            textBox34.Clear();
            textBox35.Clear();
            textBox36.Clear();
            textBox37.Text = "file path";


            MessageBox.Show("User added!");

            TbcMain.SelectedTab = AdminProfile;
        }

        private void button176_Click(object sender, EventArgs e)
        {
            textBox29.Clear();
            textBox33.Clear();
            textBox34.Clear();
            textBox35.Clear();
            textBox36.Clear();
            textBox37.Text = "file path";
        }

        private async void InsertUser()
        {
            string sourceFilePath = textBox37.Text.Trim();
            string firstName = textBox29.Text;
            string lastName = textBox33.Text;
            string contactNo = textBox34.Text;
            string username = textBox35.Text;
            string password = textBox36.Text; // Assume this is already hashed
            try
            {
                AddUserResponse response = await api_int.UserRoute.AddUser(firstName, lastName, username, password, "false", contactNo, sourceFilePath);

            }
            catch
            {
                MessageBox.Show("No image selected. Please choose an image to continue.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void panel54_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Get the list of dropped files
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Check if the first file is supported
                if (files.Length > 0 && IsSupportedFile1(files[0]))
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void panel54_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Get the list of dropped files
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Check if the first file is supported and update the text box
                if (files.Length > 0 && IsSupportedFile1(files[0]))
                {
                    textBox38.Text = files[0];
                }
                else
                {
                    MessageBox.Show("Unsupported file type.");
                }
            }
        }

        private void button26_Click(object sender, EventArgs e)
        {
            InsertKid();
            textBox43.Clear();
            textBox42.Clear();
            textBox41.Clear();
            textBox40.Clear();
            textBox39.Clear();
            textBox38.Text = "file path";
            
            
                MessageBox.Show("Child added!");
            
            TbcMain.SelectedTab = AdminProfile;
        }

        private void button174_Click(object sender, EventArgs e)
        {
            textBox43.Clear();
            textBox42.Clear();
            textBox41.Clear();
            textBox40.Clear();
            textBox39.Clear();
            textBox38.Text = "file path";
        }

        private async void InsertKid()
        {

            string firstName = textBox43.Text;
            string lastName = textBox42.Text;
            string classname = textBox41.Text;
            string notes = textBox40.Text;
            string profilePicturePath = textBox38.Text;
            string currentUser = textBox39.Text;
            string currentuser = textBox39.Text;

            try
            {
                AddChildResponse response = await api_int.ChildRoute.AddChild(firstName, lastName, classname, notes, profilePicturePath, currentUser, currentuser);

            }
            catch
            {
                MessageBox.Show("No image selected. Please choose an image to continue.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //TODO: Send Forgot Password Link
        private void LblForgotPassword_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = ForgotPassword;
        }

        private async void button27_Click(object sender, EventArgs e)
        {
            string username = textBox51.Text;
            string newpassword = textBox50.Text;

            ForgotPasswordResponse res = await api_int.UserRoute.ForgotPassword(username, newpassword);
            if (res.Code == 404)
            {
                MessageBox.Show("Username not found! Please check the entered username.", "User Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (res.Code == 500)
            {
                MessageBox.Show("An internal error occurred: " + res.Message + ". Please try again later.", "Server Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Password reset successful! You can now log in.", "Reset Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TbcMain.SelectedTab = LoginPage;
            }
            textBox51.Clear();
            textBox50.Clear();
            TBPassword.Clear();
        }

        private async void button24_Click(object sender, EventArgs e)
        {
            string title = textBox47.Text;
            string description = textBox46.Text;
            string image = textBox48.Text;

            

            AddNewsletterResponse res = await api_int.NewsletterRoute.AddNewsletter(title, description, image);

            if (res.Code == 404)
            {
                MessageBox.Show("No results found. Please check your input.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (res.Code == 500)
            {
                MessageBox.Show("An error occurred. Please re-enter and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (res.Code == 200 || res.Code == 201) // Confirming success codes
            {
                MessageBox.Show("Event Added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Unexpected response code: " + res.Code, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


            textBox47.Text = "Add event to Newseletter";
            textBox46.Clear();
            textBox48.Text = "file path";
        }

        private void panel24_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0 && IsSupportedFile(files[0]))
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void panel24_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0 && IsSupportedFiles(files[0]))
                {
                    textBox48.Text = files[0];
                }
            }
        }

        private bool IsSupportedFiles(string filePath)
        {
            string[] videoExtensions = { ".mp4", ".avi", ".mov", ".wmv" };
            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".pdf" };
            string[] audioExtensions = { ".mp3", ".wav", ".aac", ".flac", ".ogg" };
            string extension = Path.GetExtension(filePath).ToLower();

            return videoExtensions.Contains(extension) || imageExtensions.Contains(extension) || audioExtensions.Contains(extension);

        }

        private void pictureBox14_Click_1(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminAddNewsletter;
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminViewNewsletter;
        }

        private void button28_Click(object sender, EventArgs e)
        {
            // Ensure an email is selected in listBox2
            int selectedIndex = listBox2.SelectedIndex;
            if (selectedIndex != -1)
            {
                // Retrieve the selected email
                Emails selectedEmail = emails[selectedIndex];

                // Switch to the AdminCompose tab
                TbcMain.SelectedTab = AdminCompose;

                // Set the sender username in combobox4
                comboBox4.Text = selectedEmail.sender_username;

                // Set the subject in textbox23
                textBox23.Text = "re: " + selectedEmail.subject;
            }
            else
            {
                MessageBox.Show("Please select an email to reply to.", "Email Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        //USER NAVIGATION

        private void button84_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Inbox;
            LoadEmails();
        }

        private void button83_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = MessageRequest;
        }

        private void button82_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Resources;
        }

        private void button81_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Calendar;
        }

        private async void button80_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Gallery;
            GetYearResponse res = await api_int.GalleryRoute.GetYears();
            if (res.Code == 500)
            {
                MessageBox.Show("Internal server error");
            }
            else
            {
                cmbYear.Items.Clear();
                res.Years.ForEach(year => cmbYear.Items.Add(year));
            }
        }

        private void button79_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Newsletter;
        }

        private async void button78_Click(object sender, EventArgs e)
        {
            GetChildrenResponse children = await this.api_int.ChildRoute.GetChild(this.user.username);
            this.children = children.Children;

            comboBox3.Items.Clear();

            comboBox3.Items.Add($"{user.first_name} {user.last_name} (You)");

            TbcMain.SelectedTab = Profile;
            comboBox3.SelectedIndex = 0;

            if (children.Code == 404)
            {
                MessageBox.Show("This user has no children.", "No Children", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            foreach (Child child in this.children)
            {
                comboBox3.Items.Add($"{child.first_name} {child.last_name}");
            }
        }
        private void button29_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Inbox;
            LoadEmails();
        }

        private void button30_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = MessageRequest;
        }

        private void button31_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Resources;
        }

        private void button32_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Calendar;
        }

        private async void button33_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Gallery;
            GetYearResponse res = await api_int.GalleryRoute.GetYears();
            if (res.Code == 500)
            {
                MessageBox.Show("Internal server error");
            }
            else
            {
                cmbYear.Items.Clear();
                res.Years.ForEach(year => cmbYear.Items.Add(year));
            }
        }

        private void button34_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Newsletter;
        }

        private async void button35_Click(object sender, EventArgs e)
        {
            GetChildrenResponse children = await this.api_int.ChildRoute.GetChild(this.user.username);
            this.children = children.Children;

            comboBox3.Items.Clear();

            comboBox3.Items.Add($"{user.first_name} {user.last_name} (You)");

            TbcMain.SelectedTab = Profile;
            comboBox3.SelectedIndex = 0;

            if (children.Code == 404)
            {
                MessageBox.Show("This user has no children.", "No Children", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            foreach (Child child in this.children)
            {
                comboBox3.Items.Add($"{child.first_name} {child.last_name}");
            }
        }

        private void button42_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Inbox;
            LoadEmails();
        }

        private void button41_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = MessageRequest;
        }

        private void button40_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Resources;
        }

        private void button39_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Calendar;
        }

        private async void button38_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Gallery;
            GetYearResponse res = await api_int.GalleryRoute.GetYears();
            if (res.Code == 500)
            {
                MessageBox.Show("Internal server error");
            }
            else
            {
                cmbYear.Items.Clear();
                res.Years.ForEach(year => cmbYear.Items.Add(year));
            }
        }

        private void button37_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Newsletter;
        }

        private async void button36_Click(object sender, EventArgs e)
        {
            GetChildrenResponse children = await this.api_int.ChildRoute.GetChild(this.user.username);
            this.children = children.Children;

            comboBox3.Items.Clear();

            comboBox3.Items.Add($"{user.first_name} {user.last_name} (You)");

            TbcMain.SelectedTab = Profile;
            comboBox3.SelectedIndex = 0;

            if (children.Code == 404)
            {
                MessageBox.Show("This user has no children.", "No Children", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            foreach (Child child in this.children)
            {
                comboBox3.Items.Add($"{child.first_name} {child.last_name}");
            }
        }

        private void button49_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Inbox;
            LoadEmails();
        }

        private void button48_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = MessageRequest;
        }

        private void button47_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Resources;
        }

        private void button46_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Calendar;
        }

        private async void button45_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Gallery;
            GetYearResponse res = await api_int.GalleryRoute.GetYears();
            if (res.Code == 500)
            {
                MessageBox.Show("Internal server error");
            }
            else
            {
                cmbYear.Items.Clear();
                res.Years.ForEach(year => cmbYear.Items.Add(year));
            }
        }

        private void button44_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Newsletter;
        }

        private async void button43_Click(object sender, EventArgs e)
        {
            GetChildrenResponse children = await this.api_int.ChildRoute.GetChild(this.user.username);
            this.children = children.Children;

            comboBox3.Items.Clear();

            comboBox3.Items.Add($"{user.first_name} {user.last_name} (You)");

            TbcMain.SelectedTab = Profile;
            comboBox3.SelectedIndex = 0;

            if (children.Code == 404)
            {
                MessageBox.Show("This user has no children.", "No Children", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            foreach (Child child in this.children)
            {
                comboBox3.Items.Add($"{child.first_name} {child.last_name}");
            }
        }

        private void button56_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Inbox;
            LoadEmails();
        }

        private void button55_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = MessageRequest;
        }

        private void button54_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Resources;
        }

        private void button53_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Calendar;
        }

        private async void button52_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Gallery;
            GetYearResponse res = await api_int.GalleryRoute.GetYears();
            if (res.Code == 500)
            {
                MessageBox.Show("Internal server error");
            }
            else
            {
                cmbYear.Items.Clear();
                res.Years.ForEach(year => cmbYear.Items.Add(year));
            }
        }

        private void button51_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Newsletter;
        }

        private async void button50_Click(object sender, EventArgs e)
        {
            GetChildrenResponse children = await this.api_int.ChildRoute.GetChild(this.user.username);
            this.children = children.Children;

            comboBox3.Items.Clear();

            comboBox3.Items.Add($"{user.first_name} {user.last_name} (You)");

            TbcMain.SelectedTab = Profile;
            comboBox3.SelectedIndex = 0;

            if (children.Code == 404)
            {
                MessageBox.Show("This user has no children.", "No Children", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            foreach (Child child in this.children)
            {
                comboBox3.Items.Add($"{child.first_name} {child.last_name}");
            }
        }

        private void button63_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Inbox;
            LoadEmails();
        }

        private void button62_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = MessageRequest;
        }

        private void button61_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Resources;
        }

        private void button60_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Calendar;
        }

        private async void button59_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Gallery;
            GetYearResponse res = await api_int.GalleryRoute.GetYears();
            if (res.Code == 500)
            {
                MessageBox.Show("Internal server error");
            }
            else
            {
                cmbYear.Items.Clear();
                res.Years.ForEach(year => cmbYear.Items.Add(year));
            }
        }

        private void button58_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Newsletter;
        }

        private async void button57_Click(object sender, EventArgs e)
        {
            GetChildrenResponse children = await this.api_int.ChildRoute.GetChild(this.user.username);
            this.children = children.Children;

            comboBox3.Items.Clear();

            comboBox3.Items.Add($"{user.first_name} {user.last_name} (You)");

            TbcMain.SelectedTab = Profile;
            comboBox3.SelectedIndex = 0;

            if (children.Code == 404)
            {
                MessageBox.Show("This user has no children.", "No Children", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            foreach (Child child in this.children)
            {
                comboBox3.Items.Add($"{child.first_name} {child.last_name}");
            }
        }

        private void button70_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Inbox;
            LoadEmails();
        }

        private void button69_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = MessageRequest;
        }

        private void button68_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Resources;
        }

        private void button67_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Calendar;
        }

        private async void button66_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Gallery;
            GetYearResponse res = await api_int.GalleryRoute.GetYears();
            if (res.Code == 500)
            {
                MessageBox.Show("Internal server error");
            }
            else
            {
                cmbYear.Items.Clear();
                res.Years.ForEach(year => cmbYear.Items.Add(year));
            }
        }

        private void button65_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Newsletter;
        }

        private async void button64_Click(object sender, EventArgs e)
        {
            GetChildrenResponse children = await this.api_int.ChildRoute.GetChild(this.user.username);
            this.children = children.Children;

            comboBox3.Items.Clear();

            comboBox3.Items.Add($"{user.first_name} {user.last_name} (You)");

            TbcMain.SelectedTab = Profile;
            comboBox3.SelectedIndex = 0;

            if (children.Code == 404)
            {
                MessageBox.Show("This user has no children.", "No Children", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            foreach (Child child in this.children)
            {
                comboBox3.Items.Add($"{child.first_name} {child.last_name}");
            }
        }

        private void button77_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Inbox;
            LoadEmails();
        }

        private void button76_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = MessageRequest;
        }

        private void button75_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Resources;
        }

        private void button74_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Calendar;
        }

        private async void button73_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Gallery;
            GetYearResponse res = await api_int.GalleryRoute.GetYears();
            if (res.Code == 500)
            {
                MessageBox.Show("Internal server error");
            }
            else
            {
                cmbYear.Items.Clear();
                res.Years.ForEach(year => cmbYear.Items.Add(year));
            }
        }

        private void button72_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Newsletter;
        }

        private async void button71_Click(object sender, EventArgs e)
        {
            GetChildrenResponse children = await this.api_int.ChildRoute.GetChild(this.user.username);
            this.children = children.Children;

            comboBox3.Items.Clear();

            comboBox3.Items.Add($"{user.first_name} {user.last_name} (You)");

            TbcMain.SelectedTab = Profile;
            comboBox3.SelectedIndex = 0;

            if (children.Code == 404)
            {
                MessageBox.Show("This user has no children.", "No Children", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            foreach (Child child in this.children)
            {
                comboBox3.Items.Add($"{child.first_name} {child.last_name}");
            }
        }

        private void button91_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Inbox;
            LoadEmails();
        }

        private void button90_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = MessageRequest;
        }

        private void button89_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Resources;
        }

        private void button88_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Calendar;
        }

        private async void button87_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Gallery;
            GetYearResponse res = await api_int.GalleryRoute.GetYears();
            if (res.Code == 500)
            {
                MessageBox.Show("Internal server error");
            }
            else
            {
                cmbYear.Items.Clear();
                res.Years.ForEach(year => cmbYear.Items.Add(year));
            }
        }

        private void button86_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Newsletter;
        }

        private async void button85_Click(object sender, EventArgs e)
        {
            GetChildrenResponse children = await this.api_int.ChildRoute.GetChild(this.user.username);
            this.children = children.Children;

            comboBox3.Items.Clear();

            comboBox3.Items.Add($"{user.first_name} {user.last_name} (You)");

            TbcMain.SelectedTab = Profile;
            comboBox3.SelectedIndex = 0;

            if (children.Code == 404)
            {
                MessageBox.Show("This user has no children.", "No Children", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            foreach (Child child in this.children)
            {
                comboBox3.Items.Add($"{child.first_name} {child.last_name}");
            }
        }

        private void button98_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Inbox;
            LoadEmails();
        }

        private void button97_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = MessageRequest;
        }

        private void button96_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Resources;
        }

        private void button95_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Calendar;
        }

        private async void button94_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Gallery;
            GetYearResponse res = await api_int.GalleryRoute.GetYears();
            if (res.Code == 500)
            {
                MessageBox.Show("Internal server error");
            }
            else
            {
                cmbYear.Items.Clear();
                res.Years.ForEach(year => cmbYear.Items.Add(year));
            }
        }

        private void button93_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Newsletter;
        }

        private async void button92_Click(object sender, EventArgs e)
        {
            GetChildrenResponse children = await this.api_int.ChildRoute.GetChild(this.user.username);
            this.children = children.Children;

            comboBox3.Items.Clear();

            comboBox3.Items.Add($"{user.first_name} {user.last_name} (You)");

            TbcMain.SelectedTab = Profile;
            comboBox3.SelectedIndex = 0;

            if (children.Code == 404)
            {
                MessageBox.Show("This user has no children.", "No Children", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            foreach (Child child in this.children)
            {
                comboBox3.Items.Add($"{child.first_name} {child.last_name}");
            }
        }

        private void button105_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Inbox;
            LoadEmails();
        }

        private void button104_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = MessageRequest;
        }

        private void button103_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Resources;
        }

        private void button102_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Calendar;
        }

        private async void button101_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Gallery;
            GetYearResponse res = await api_int.GalleryRoute.GetYears();
            if (res.Code == 500)
            {
                MessageBox.Show("Internal server error");
            }
            else
            {
                cmbYear.Items.Clear();
                res.Years.ForEach(year => cmbYear.Items.Add(year));
            }
        }

        private void button100_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Newsletter;
        }

        private async void button99_Click(object sender, EventArgs e)
        {
            GetChildrenResponse children = await this.api_int.ChildRoute.GetChild(this.user.username);
            this.children = children.Children;

            comboBox3.Items.Clear();

            comboBox3.Items.Add($"{user.first_name} {user.last_name} (You)");

            TbcMain.SelectedTab = Profile;
            comboBox3.SelectedIndex = 0;

            if (children.Code == 404)
            {
                MessageBox.Show("This user has no children.", "No Children", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            foreach (Child child in this.children)
            {
                comboBox3.Items.Add($"{child.first_name} {child.last_name}");
            }
        }

        private void button112_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Inbox;
            LoadEmails();
        }

        private void button111_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = MessageRequest;
        }

        private void button110_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Resources;
        }

        private void button109_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Calendar;
        }

        private async void button108_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Gallery;
            GetYearResponse res = await api_int.GalleryRoute.GetYears();
            if (res.Code == 500)
            {
                MessageBox.Show("Internal server error");
            }
            else
            {
                cmbYear.Items.Clear();
                res.Years.ForEach(year => cmbYear.Items.Add(year));
            }
        }

        private void button107_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Newsletter;
        }

        private async void button106_Click(object sender, EventArgs e)
        {
            GetChildrenResponse children = await this.api_int.ChildRoute.GetChild(this.user.username);
            this.children = children.Children;

            comboBox3.Items.Clear();

            comboBox3.Items.Add($"{user.first_name} {user.last_name} (You)");

            TbcMain.SelectedTab = Profile;
            comboBox3.SelectedIndex = 0;

            if (children.Code == 404)
            {
                MessageBox.Show("This user has no children.", "No Children", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            foreach (Child child in this.children)
            {
                comboBox3.Items.Add($"{child.first_name} {child.last_name}");
            }
        }

        private void pictureBox57_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = MessageRequest;
            textBox20.Text = "Subject:";
            textBox19.Text = "Message:";
            comboBox5.Text = "To:";
        }

        private async void pictureBox53_Click(object sender, EventArgs e)
        {
            GetChildrenResponse children = await this.api_int.ChildRoute.GetChild(this.user.username);
            this.children = children.Children;

            comboBox3.Items.Clear();

            comboBox3.Items.Add($"{user.first_name} {user.last_name} (You)");

            TbcMain.SelectedTab = Profile;
            comboBox3.SelectedIndex = 0;

            if (children.Code == 404)
            {
                MessageBox.Show("This user has no children.", "No Children", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            foreach (Child child in this.children)
            {
                comboBox3.Items.Add($"{child.first_name} {child.last_name}");
            }
        }

        private void pictureBox54_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminProfile;
        }

        private async void pictureBox5_Click(object sender, EventArgs e)
        {
            GetChildrenResponse children = await this.api_int.ChildRoute.GetChild(this.user.username);
            this.children = children.Children;

            comboBox3.Items.Clear();

            comboBox3.Items.Add($"{user.first_name} {user.last_name} (You)");

            TbcMain.SelectedTab = Profile;
            comboBox3.SelectedIndex = 0;

            if (children.Code == 404)
            {
                MessageBox.Show("This user has no children.", "No Children", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            foreach (Child child in this.children)
            {
                comboBox3.Items.Add($"{child.first_name} {child.last_name}");
            }
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Resources;
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = Resources;
        }


        //ADMIN NAVIGATION
        private void button121_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminInbox;
            LoadEmails2();
        }

        private void button120_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = UserSearch;
            LoadAllKids();
        }

        private void button119_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminResources;
        }

        private void button118_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminGallery;
        }

        private void button117_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminCalendar;
        }

        private void button116_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminNewsletter;
        }

        private void button115_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminProfile;
        }

        private void button128_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminInbox;
            LoadEmails2();
        }

        private void button127_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = UserSearch;
            LoadAllKids();
        }

        private void button126_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminResources;
        }

        private void button125_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminGallery;
        }

        private void button124_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminCalendar;
        }

        private void button123_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminNewsletter;
        }

        private void button122_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminProfile;
        }

        private void button135_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminInbox;
            LoadEmails2();
        }

        private void button134_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = UserSearch;
            LoadAllKids();
        }

        private void button133_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminResources;
        }

        private void button132_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminGallery;
        }

        private void button131_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminCalendar;
        }

        private void button130_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminNewsletter;
        }

        private void button129_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminProfile;
        }

        private void button142_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminInbox;
            LoadEmails2();
        }

        private void button141_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = UserSearch;
            LoadAllKids();
        }

        private void button140_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminResources;
        }

        private void button139_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminGallery;
        }

        private void button138_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminCalendar;
        }

        private void button137_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminNewsletter;
        }

        private void button136_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminProfile;
        }

        private void button149_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminInbox;
            LoadEmails2();
        }

        private void button148_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = UserSearch;
            LoadAllKids();
        }

        private void button147_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminResources;
        }

        private void button146_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminGallery;
        }

        private void button145_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminCalendar;
        }

        private void button144_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminNewsletter;
        }

        private void button143_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminProfile;
        }

        private void button156_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminInbox;
            LoadEmails2();
        }

        private void button155_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = UserSearch;
            LoadAllKids();
        }

        private void button154_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminResources;
        }

        private void button153_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminGallery;
        }

        private void button152_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminCalendar;
        }

        private void button151_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminNewsletter;
        }

        private void button150_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminProfile;
        }

        private void pictureBox55_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminInbox;
            LoadEmails2();
            textBox23.Text = "Subject:";
            textBox22.Text = "Message";
            comboBox4.Text = "To:";
        }

        private void button163_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminInbox;
            LoadEmails2();
        }

        private void button162_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = UserSearch;
            LoadAllKids();
        }

        private void button161_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminResources;
        }

        private void button160_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminGallery;
        }

        private void button159_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminCalendar;
        }

        private void button158_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminNewsletter;
        }

        private void button157_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminProfile;
        }

        private void pictureBox59_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminResources;
        }

        private void pictureBox60_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminGallery;
        }

        private void pictureBox61_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminCalendar;
        }

        private void pictureBox62_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminProfile;
        }

        private void pictureBox63_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminProfile;
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminNewsletter;
        }

        private void button170_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminInbox;
            LoadEmails2();
        }

        private void button169_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = UserSearch;
            LoadAllKids();
        }

        private void button168_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminResources;
        }

        private void button167_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminGallery;
        }

        private void button166_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminCalendar;
        }

        private void button165_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminNewsletter;
        }

        private void button164_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminProfile;
        }

        private void pictureBox88_Click_1(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminViewResources;
        }

        private void pictureBox81_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminResources;
        }

        private void pictureBox85_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminCurrentMedia;
        }

        private void pictureBox69_Click_1(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminViewResources;
        }

        private void pictureBox92_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminViewResources;
        }

        private void pictureBox96_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminViewResources;
        }

        private void pictureBox101_Click_1(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminViewResources;
        }

        private void pictureBox82_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminCurrentArts;
        }

        private void pictureBox84_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminCurrentTips;
        }

        private void pictureBox83_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminCurrentAdditional;
        }

        private void pictureBox108_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminGallery;
        }

        private void pictureBox115_Click_1(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminCurrentGallery;
        }

        private void pictureBox124_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminCalendar;
        }

        private void pictureBox133_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminNewsletter;
        }

        private void pictureBox66_Click_1(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = AdminProfile;
        }

        private void label6_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = UserMedia;
        }

        private void label7_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = UserArtsandCrafts;
        }

        private void label8_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = UserTips;
        }

        private void label9_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = UserResources;
        }

        private void listBox6_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Make sure the index is valid
            if (e.Index < 0) return;

            // Get the item text
            string itemText = listBox6.Items[e.Index].ToString();

            // Determine the size of the text
            SizeF textSize = e.Graphics.MeasureString(itemText, e.Font);

            // Calculate the position to draw the text centered
            float x = (e.Bounds.Width - textSize.Width) / 2;
            float y = (e.Bounds.Height - textSize.Height) / 2;

            // Clear the background
            e.DrawBackground();

            // Draw the text
            e.Graphics.DrawString(itemText, e.Font, new SolidBrush(e.ForeColor), new PointF(x, y));

            // Draw the focus rectangle if needed
            e.DrawFocusRectangle();
        }

        private void listBox4_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Make sure the index is valid
            if (e.Index < 0) return;

            // Get the item text
            string itemText = listBox4.Items[e.Index].ToString();

            // Determine the size of the text
            SizeF textSize = e.Graphics.MeasureString(itemText, e.Font);

            // Calculate the position to draw the text centered
            float x = (e.Bounds.Width - textSize.Width) / 2;
            float y = (e.Bounds.Height - textSize.Height) / 2;

            // Clear the background
            e.DrawBackground();

            // Draw the text
            e.Graphics.DrawString(itemText, e.Font, new SolidBrush(e.ForeColor), new PointF(x, y));

            // Draw the focus rectangle if needed
            e.DrawFocusRectangle();
        }

        private void listBox5_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Make sure the index is valid
            if (e.Index < 0) return;

            // Get the item text
            string itemText = listBox5.Items[e.Index].ToString();

            // Determine the size of the text
            SizeF textSize = e.Graphics.MeasureString(itemText, e.Font);

            // Calculate the position to draw the text centered
            float x = (e.Bounds.Width - textSize.Width) / 2;
            float y = (e.Bounds.Height - textSize.Height) / 2;

            // Clear the background
            e.DrawBackground();

            // Draw the text
            e.Graphics.DrawString(itemText, e.Font, new SolidBrush(e.ForeColor), new PointF(x, y));

            // Draw the focus rectangle if needed
            e.DrawFocusRectangle();
        }

        private void listBox7_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Make sure the index is valid
            if (e.Index < 0) return;

            // Get the item text
            string itemText = listBox7.Items[e.Index].ToString();

            // Determine the size of the text
            SizeF textSize = e.Graphics.MeasureString(itemText, e.Font);

            // Calculate the position to draw the text centered
            float x = (e.Bounds.Width - textSize.Width) / 2;
            float y = (e.Bounds.Height - textSize.Height) / 2;

            // Clear the background
            e.DrawBackground();

            // Draw the text
            e.Graphics.DrawString(itemText, e.Font, new SolidBrush(e.ForeColor), new PointF(x, y));

            // Draw the focus rectangle if needed
            e.DrawFocusRectangle();
        }

        private void pictureBox26_Click(object sender, EventArgs e)
        {
            TbcMain.SelectedTab = LoginPage;
            TBUsername.Clear();
            TBPassword.Clear();
            comboBox3.Items.Clear();
        }
    }
}