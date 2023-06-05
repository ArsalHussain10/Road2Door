function showLicenseModal(license) {
    var imageUrl = license;
    console.log(imageUrl);
    document.getElementById("licenseModal").style.display = "block";
    document.getElementById("licenseImage").src = imageUrl;
}

function hideLicenseModal() {
    document.getElementById("licenseModal").style.display = "none";
}


function toggleAccountStatus(riderId, accountStatus) {

    if (accountStatus === "1") {
        confirmationMessage = "Are you sure you want to deactivate  this account?";
        accountStatus = "0";
    } else {
        confirmationMessage = "Are you sure you want to activate this account?";
        accountStatus = "1"
    }
    if (confirm(confirmationMessage)) {
        window.location.href = "/Admin/ChangeAccountStatusRider?riderId=" + riderId + "&accountStatus=" + accountStatus;

    }



}