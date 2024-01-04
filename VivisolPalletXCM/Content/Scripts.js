////function mySubmit(s, e) {
////    if (idDocumentoTextBox.GetText() != "") {
////        $('#myForm').submit();
////    }
////}

function mySubmit(s, e) {
    console.log("submit");
    $.ajax({
        type: "POST",
        url: "/Home/Index",
        data: { idDocumento: idDocumentoTextBox.GetText(), numeroConsegna: numeroConsegnaTextBox.GetText() },
        dataType: "text",
        success: function (response) {
            idDocumentoTextBox.SetText("");
            numeroConsegnaTextBox.SetText(parseInt(numeroConsegnaTextBox.GetText()) + 1);
            idDocumentoTextBox.Focus();
    
        }

    });
};