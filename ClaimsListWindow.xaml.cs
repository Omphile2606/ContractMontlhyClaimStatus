using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.IO;
using ContractMontlhyClaimStatus.Repositories;
using ContractMonthlyClaimSystem.Models;


namespace ContractMontlhyClaimStatus
{
    /// <summary>
    /// Interaction logic for ClaimsListWindow.xaml
    /// </summary>
    public partial class ClaimsListWindow : Window
    {
        public ClaimsListWindow()
        {
            InitializeComponent();
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            DgClaims.ItemsSource = null;
            DgClaims.ItemsSource = ClaimRepository.GetAll();
        }

        private Claim GetClaimFromSender(object sender)
        {
            if (sender is Button btn && btn.DataContext is Claim claim)
                return claim;
            // fallback: use selected item
            return DgClaims.SelectedItem as Claim;
        }

        private void BtnApprove_Click(object sender, RoutedEventArgs e)
        {
            var claim = GetClaimFromSender(sender);
            if (claim == null) return;

            ClaimRepository.Approve(claim.Id);
            RefreshGrid();
        }

        private void BtnReject_Click(object sender, RoutedEventArgs e)
        {
            var claim = GetClaimFromSender(sender);
            if (claim == null) return;

            ClaimRepository.Reject(claim.Id);
            RefreshGrid();
        }

        private void BtnOpenDoc_Click(object sender, RoutedEventArgs e)
        {
            var claim = GetClaimFromSender(sender);
            if (claim == null) return;

            if (string.IsNullOrEmpty(claim.DocumentFullPath) || !File.Exists(claim.DocumentFullPath))
            {
                MessageBox.Show("No document available for this claim.", "Open Document", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                Process.Start(new ProcessStartInfo { FileName = claim.DocumentFullPath, UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to open document: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e) => RefreshGrid();

        private void BtnClose_Click(object sender, RoutedEventArgs e) => Close();
    }
}