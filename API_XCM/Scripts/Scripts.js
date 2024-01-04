function logoutSubmit(s, e) {
    console.log("submit");
    $.ajax({
        type: "GET",
        url: "/Home/Logout",
        success: function (response) {
            window.location.href = "/Home/Index";
        },
        failure: function (response) {
            console.log(response);
        },
        error: function (response) {
            console.log(response);
        }

    });
};


function OnToolbarItemClick(s, e) {
    $.ajax({
        type: "GET",
        url: "/Nino/RefreshGridView",
        success: function (response) {
            console.log(response)
        },
        failure: function (response) {
            console.log(response);
        },
        error: function (response) {
            console.log(response);
        }

    });
    //$('form').submit();
    //window.setTimeout(function () {
    //    $exportFormat.val("");
    //}, 0);
}



