function showDocumentModal(license) {
    var imageUrl = license;
    console.log(imageUrl);
    document.getElementById("licenseModal").style.display = "block";
    document.getElementById("licenseImage").src = imageUrl;
}

function hideDocumentModal() {
    document.getElementById("licenseModal").style.display = "none";
}
function acceptRequest(riderId) {
    // Perform necessary actions for accepting the request
    accountRequest = 1;
    window.location.href = "/Admin/AccountRequest?riderId=" + riderId + "&accountRequest=" + accountRequest;

}

function rejectRequest(riderId) {
    // Perform necessary actions for rejecting the request
    accountRequest = 0;
    window.location.href = "/Admin/AccountRequest?riderId=" + riderId + "&accountRequest=" + accountRequest;
}