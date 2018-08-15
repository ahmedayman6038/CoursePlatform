$(document).ready(function () {
    var form = $('#myForm'); // contact form
    var submit = $('.submit-btn'); // submit button
    var alert = $('.alert-msg'); // alert div for show alert message

    // form submit event
    form.on('submit', function (e) {
        e.preventDefault(); // prevent default form submit
        if (form.valid()) {
            $.ajax({
                url: '/Home/Contact', // form action url
                type: 'POST', // form submit method get/post
                dataType: 'html', // request type html/json/xml
                data: form.serialize(), // serialize form data
                beforeSend: function () {
                    alert.fadeOut();
                    submit.html('ارسال ...'); // change submit button text
                },
                success: function (data) {
                    alert.html(data).fadeIn(); // fade in response data
                    form.trigger('reset'); // reset form
                    submit.attr("style", "display: none !important"); // reset submit button text
                },
                error: function (e) {
                    console.log(e)
                }
            });
        }

    });
    function GetStudent() {
        var len = $("#StudentCode").val().length;
        if (len == 14) {
            var code = $("#StudentCode").val();
            $.ajax({
                type: 'GET',
                url: '/Student/GetStudent',
                data: { "code": code },
                dataType: 'json',
                success: function (data) {
                    $("#StudentName").val(data.StudentName);
                    $("#StudentFaculty").val(data.FacultyName);
                    $('#code-error').html('');
                },
                complete: function () {

                },
                error: function () {
                    $('#code-error').html('كود الطالب غير صحيح');
                    $("#StudentName").val('');
                    $("#StudentFaculty").val('');
                }
            });
        }
      
    }
    //GetStudent();
    $("#StudentCode").keyup(function () {
        GetStudent();
    });
    $("#StudentCode").blur(function () {
        GetStudent();
    });
})