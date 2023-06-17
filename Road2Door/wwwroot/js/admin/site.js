function uploadFiles() {
    var formData = new FormData(); //create form
    formData.append("name", $("#name").val());
    formData.append("email", $("#email").val());
    formData.append("password", $("#password").val());
    formData.append("phone", $("#phone").val());
    formData.append("cnic", $("#cnic").val());
    formData.append("file1", $("#license")[0].files[0]);
    formData.append("file2", $("#criminal-record")[0].files[0]);
    //alert("Before ajax");

    $.ajax(
        {
            url: "/Rider/SignUp",
            data: formData, // send it as formData
            processData: false,// tell jQuery not to process the data
            contentType: false,
            type: "POST", //type is post as we will need to post files
            success: function () {
                alert("Files Uploaded!");
            }
            
        }
    );
    //alert("After ajax");


}