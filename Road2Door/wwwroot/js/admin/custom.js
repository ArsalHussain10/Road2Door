
/*--------------------------------------
	sidebar
--------------------------------------*/

"use strict";

$(document).ready(function () {
    $('#sidebarCollapse').on('click', function () {
        if ($('#sidebar').hasClass('active')) {
            $('#sidebar').removeClass('active');
            $('.topbar').css('padding-left', '280px');
        } else {
            $('#sidebar').addClass('active');
            $('.topbar').css('padding-left', '90px');
        }
    });

});

