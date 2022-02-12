// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//$.extend.methods.number = function (value, element) {
//    return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
//}

jQuery.extend(jQuery.validator.methods, {
    date: function (value, element) {
        return this.optional(element) || /^\d\d?\/\d\d?\/\d\d\d?\d?$/.test(value);
    },
    number: function (value, element) {
        return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:\.\d{3})+)(?:,\d+)?$/.test(value);
    }
});

$(document).ready(function () {

    //Filtro Decimal para aceitar vírgula
    $.validator.methods.range = function (value, element, param) {
        var globalizedValue = value.replace(",", ".");
        return this.optional(element) || (globalizedValue >= param[0] && globalizedValue <= param[1]);
    }

    $.validator.methods.number = function (value, element) {
        return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
    }
});


function ResgatarEnderecoCEP(pInputCep) {
    if (pInputCep.value.length == 8) {
        $.ajax({
            type: "get",
            url: "https://brasilapi.com.br/api/cep/v1/" + pInputCep.value,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result) {
                    document.getElementById("Endereco").value = result.street + ", " +
                        result.neighborhood + ", " +
                        result.city + ", " +
                        result.state;
                }
            },
            error: function (xhr, status, error) {
                alert("CEP não localizado")
            }
        });
    }
}