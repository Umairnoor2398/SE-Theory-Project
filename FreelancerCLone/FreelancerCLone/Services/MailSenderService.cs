using System.Net.Mail;
using System.Net;
using FreelancerCLone.Utilities;
using FreelancerCLone.DbModels;

namespace FreelancerCLone.Services
{
    // Service class responsible for sending emails in the FreelancerClone application
	public class MailSenderService
    {
        private static MailSenderService _instance;
        private static SmtpClient smtpClient;

        private static string emailAddress = "";
        private static string password = "";

		// Singleton pattern: Ensures only one instance of the MailSenderService is created
		public static MailSenderService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MailSenderService();
                    smtpClient = new SmtpClient();
                    smtpClient.Port = 587;
                    smtpClient.Host = "smtp.gmail.com";
                    smtpClient.EnableSsl = true;
                    smtpClient.Timeout = 30000;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(emailAddress, password);
                }
                return _instance;
            }
        }


        private MailSenderService() { }
		// Sends an email to the user for account activation on registration

		public async Task SendMailToUserOnRegister(string Email, string Name, string code)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(emailAddress);
            mail.To.Add(Email);
            mail.Subject = "Activate Your Freelancer Account";
            mail.IsBodyHtml = true;



            string content = "Dear " + Name + ",\r\n\r\nWelcome to Freelancer! We're thrilled to have you on board. To complete your registration and activate your account, please click the link below:\r\n\r\n" + code + "\r\n\r\nThank you for choosing Freelancer. If you have any questions or need assistance, feel free to contact our support team at bisma3131@gmail.com.\r\n\r\nBest regards,\r\nFreelancer";

            mail.Body = content;
            smtpClient.Send(mail);
        }

		// Sends an email to the user on receiving feedback

		public async Task SendMailToOnReceivingFeedback(string username)
        {
            var user = UserUtility.Instance.GetUserForProfile(0, username);
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(emailAddress);
            mail.To.Add(user.UserNavigation.Email);
            mail.Subject = "Thank You for Your Feedback!";
            mail.IsBodyHtml = true;



            string content = "Dear " + user.FirstName + " " + user.LastName + ",\r\n\r\nThank you for taking the time to share your feedback with us! We greatly appreciate your input, as it helps us improve and enhance the Freelancer experience.\r\n\r\nYour thoughts are valuable to us, and we want you to know that we've received your feedback. Our team will carefully review your comments and suggestions.\r\n\r\nIf you have any further details to add or if you have additional feedback in the future, please don't hesitate to reach out. We're here to listen and make Freelancer even better.\r\n\r\nThank you for being a part of our community!\r\n\r\nBest regards,\r\nFreelancer";

            mail.Body = content;
            smtpClient.Send(mail);
        }

		// Sends an email to the user after reviewing their feedback
		public async Task SendMailToOnReviewingFeedback(int userId)
        {
            var user = UserUtility.Instance.GetUserForProfile(userId, "");
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(emailAddress);
            mail.To.Add(user.UserNavigation.Email);
            mail.Subject = "Feedback Review for Your Feedback Input";
            mail.IsBodyHtml = true;



            string content = "Dear " + user.FirstName + " " + user.LastName + ",\r\n\r\nI hope this email finds you well. I wanted to personally reach out to express our gratitude for your recent feedback on Freelancer. Your insights are invaluable, and we appreciate your dedication to helping us improve.\r\n\r\nAfter careful consideration, our team has reviewed your feedback, and we want to assure you that your input is being taken into account as we work to enhance our Freelancer. Your contribution is vital to our continuous improvement efforts.\r\n\r\nThank you again for being an essential part of our community. If you have any further thoughts or suggestions, please don't hesitate to share them with us.\r\n\r\nWe look forward to providing you with an even better experience in the future.\r\n\r\nBest regards,\r\nFreelancer Team";

            mail.Body = content;
            smtpClient.Send(mail);
        }
		// Sends an email to the user notifying them of the completion and rating of their project

		public async Task SendMailRateProject(ProjectBid projectBid)
        {
            var user = UserUtility.Instance.GetUserForProfile(projectBid.UserId, "");
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(emailAddress);
            mail.To.Add(user.UserNavigation.Email);
            mail.Subject = "Congratulations! Your Project on Freelancer is Complete";
            mail.IsBodyHtml = true;



            string content = "Dear " + user.FirstName + " " + user.LastName + ",\r\n\r\nWe hope this message finds you well. We're thrilled to inform you that the project you worked on, " + projectBid.Project.Title + ", has been successfully completed, and the client has provided their Rating.\r\n\r\nClient's Rating: " + projectBid.Rating + "\r\n\r\nYour hard work and dedication have truly paid off, and the client has expressed their satisfaction with the outcome. We appreciate your commitment to delivering high-quality work through Freelancer.\r\n\r\nIf you have any questions or if there's anything else we can assist you with, please don't hesitate to reach out.\r\n\r\nThank you for your outstanding contribution to Freelancer! We look forward to more successful collaborations in the future.\r\n\r\nBest regards,\r\n\r\nFreelancer";

            mail.Body = content;
            smtpClient.Send(mail);
        }
		// Sends an email to the project owner on receiving a new bid for their project

		public async Task SendMailOnReceiveBidInProject(ProjectBid projectBid)
        {
            var user = UserUtility.Instance.GetUserForProfile(projectBid.Project.AddedBy, "");
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(emailAddress);
            mail.To.Add(user.UserNavigation.Email);
            mail.Subject = "New Bid Received for Your Project on Freelancer";
            mail.IsBodyHtml = true;



            string content = "Dear " + user.FirstName + " " + user.LastName + ",\r\n\r\nWe hope this message finds you well. We wanted to inform you that a freelancer has shown interest in your project, " + projectBid.Project.Title + ", on Freelancer. Here are the details of the bid:\r\n\r\nBidder's Username: " + projectBid.User.FirstName + " " + projectBid.User.LastName + "\r\nBid Amount: " + projectBid.BidBudget + "PKR\r\nDeadline: " + projectBid.BidDeadline + " Days\r\n\r\nThis is an exciting development, and it indicates that freelancers are eager to work on your project. You can review the bidder's profile and their proposal on the project page.\r\n\r\nIf you have any questions or if there's anything else we can assist you with, feel free to reach out. We wish you the best in finding the perfect freelancer for your project!\r\n\r\nBest regards,\r\n\r\nFreelancer Team";

            mail.Body = content;
            smtpClient.Send(mail);
        }
		// Sends an email to the freelancer on the approval of their bid for a project

		public async Task SendMailOnApproveBidInProject(ProjectBid projectBid)
        {
            var user = UserUtility.Instance.GetUserForProfile(projectBid.UserId, "");
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(emailAddress);
            mail.To.Add(user.UserNavigation.Email);
            mail.Subject = "Congratulations! Your Bid Has Been Approved";
            mail.IsBodyHtml = true;



            string content = "Dear " + user.FirstName + " " + user.LastName + ",\r\n\r\nWe're delighted to inform you that the owner of the project, " + projectBid.Project.Title + ", has approved your bid. Congratulations on being selected for this opportunity!\r\n\r\nProject Details:\r\n\r\nOwner's Username: " + projectBid.Project.AddedByNavigation.FirstName + " " + projectBid.Project.AddedByNavigation.LastName + "\r\nProject Name: " + projectBid.Project.Title + "\r\nBid Amount: " + projectBid.BidBudget + "PKR\r\nDeadline: " + projectBid.BidDeadline + " Days\r\nThis is a significant step forward, and we encourage you to initiate communication with the project owner to discuss further details and kickstart the collaboration.\r\n\r\nIf you have any questions or need assistance, feel free to reach out to us. We wish you a successful and productive collaboration on Freelancer.\r\n\r\nBest regards,\r\n\r\nFreelancer Team";

            mail.Body = content;
            smtpClient.Send(mail);
        }
		// Sends an email to the project owner on updating the completeness status of a project

		public async Task SendMailOnProjectCompletenesssUpdate(ProjectBid projectBid)
        {
            var user = UserUtility.Instance.GetUserForProfile(projectBid.Project.AddedBy, "");
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(emailAddress);
            mail.To.Add(user.UserNavigation.Email);
            mail.IsBodyHtml = true;
            string content = "";
            if (projectBid.IsCompleted == true)
            {
                mail.Subject = "Project Update: " + projectBid.Project.Title + " - Submitted for Your Review";
                content = "Dear " + user.FirstName + " " + user.LastName + ",\r\n\r\nI trust this message finds you well. I'm pleased to share an update on your project, " + projectBid.Project.Title + ". The freelancer, " + projectBid.User.FirstName + " " + projectBid.User.LastName + ", has successfully completed the work and has submitted it for your review.\r\n\r\nStatus Update:\r\n\r\nSubmission: Completed\r\nFreelancer's Username: " + projectBid.User.FirstName + " " + projectBid.User.LastName + "\r\nProject Status: Under Review\r\nWe encourage you to take the time to review the submitted work. If you have any feedback or if further revisions are needed, please communicate directly with the freelancer through our platform's messaging system.\r\n\r\nIf you have any questions or need assistance, feel free to reach out. We appreciate your collaboration on Freelancer.\r\n\r\nBest regards,\r\n\r\nFreelancer Team";
            }
            else
            {
                mail.Subject = "Project Update: " + projectBid.Project.Title + " - Further Revisions Requested";
                content = "Dear " + user.FirstName + " " + user.LastName + ",\r\n\r\nI hope this message finds you in good spirits. I'm writing to inform you about your project, " + projectBid.Project.Title + ". The freelancer, " + projectBid.User.FirstName + " " + projectBid.User.LastName + ", has requested to take the project back for further revisions to ensure it aligns perfectly with your expectations.\r\n\r\nStatus Update:\r\n\r\nSubmission: Returned for Revision\r\nFreelancer's Username: " + projectBid.User.FirstName + " " + projectBid.User.LastName + "\r\nProject Status: Under Revision\r\nYour input is crucial in refining the project, and we appreciate your collaboration. Please feel free to communicate directly with the freelancer through our platform's messaging system for any specific details or adjustments required.\r\n\r\nIf you have any questions or need assistance, don't hesitate to reach out. Thank you for your ongoing engagement on Freelancer.\r\n\r\nBest regards,\r\n\r\nFreelancer Team";
            }



            mail.Body = content;
            smtpClient.Send(mail);
        }

    }
}
