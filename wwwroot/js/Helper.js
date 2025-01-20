$(document).ready(function () {
    $("#cpf-cnpj").keydown(function () {
        try {
            $("#cpf-cnpj").unmask();
        } catch (e) { }

        var tamanho = $("#cpf-cnpj").val().length;

        if (tamanho < 11) {
            $("#cpf-cnpj").mask("999.999.999-99");
        } else {
            $("#cpf-cnpj").mask("99.999.999/9999-99");
        }

        var elem = this;
        setTimeout(function () {
            elem.selectionStart = elem.selectionEnd = 10000;
        }, 0);

        var currentValue = $(this).val();
        $(this).val('');
        $(this).val(currentValue);
    });
});