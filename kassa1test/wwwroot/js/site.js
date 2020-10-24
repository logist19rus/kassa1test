// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('#credit_form').submit(function(e) {
        var check = Validate();
        if (check == false) {
            e.preventDefault();
        }
    });
});

function Validate() {
    var rezult = true;

    var Sum = $('#Sum').val();
    var CreditTime = $('#CreditTime').val();
    var PeriodType = $('#PeriodType').val();    
    var CreditRate = $('#CreditRate').val();
    var RateType = $('#RateType').val();
    var PayPeriod = $('#PayPeriod').val();

    var regExNumeric = /^[0-9]+$/;
    var regExDouble = /^[0-9]+(.[0-9]+)?$/;

    $("small").html("");
    if (PeriodType == 'true') {
        if (RateType == 'false') {
            $('#RateType').parents('.form-group').children('small').html('При коротком сроке кредита используйте ежедневную ставку');
            rezult = false;
        }
        if (PayPeriod == 30) {
            rezult = false;
            $('#PayPeriod').parents('.form-group').children('small').html('При коротком сроке кредита не выберайте ежемесячный шаг платежа');
        }
    }

    if (Sum.length < 1) {
        $('#Sum').parents('.form-group').children('small').html('Пустое поле');
        rezult = false;
    }
    else {
        var validSum = regExNumeric.test(Sum);
        if (!validSum) {
            $('#Sum').parents('.form-group').children('small').html('Поле должно состоять из цифр');
            rezult = false;
        }
    }
    if (CreditTime.length < 1) {
        $('#CreditTime').parents('.form-group').children('small').html('Пустое поле');
        rezult = false;
    }
    else {
        var validSum = regExNumeric.test(CreditTime);
        if (!validSum) {
            $('#CreditTime').parents('.form-group').children('small').html('Поле должно состоять из цифр');
            rezult = false;
        }
        else if (PayPeriod != 30 && PeriodType == 'true' && CreditTime % PayPeriod != 0) {
            $('#CreditTime').parents('.form-group').children('small').html('Число дней должно быть кратно периоду платежа');
            rezult = false;
        }
    }
    if (CreditRate.length < 1) {
        $('#CreditRate').parents('.form-group').children('small').html('Пустое поле');
        rezult = false;
    }
    else {
        var validSum = regExDouble.test(CreditRate);
        if (!validSum) {
            $('#CreditRate').parents('.form-group').children('small').html('Поле должно состоять из цифр');
            rezult = false;
        }
    }
    return rezult;
    //console.log(Sum + " " + CreditTime + " " + PeriodType + " " + CreditRate + " " + RateType + " " + PayPeriod);
}
