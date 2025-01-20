$(document).ready(function () {
    // Para CPF e CNPJ
    $("#cpf-cnpj").on("input", function () {
        var valor = $(this).val();
        var tamanho = valor.length;

        // Só muda a máscara quando o valor realmente mudar
        if (tamanho < 11) {
            if (!$(this).hasClass("cpf-mask")) {
                $(this).mask("999.999.999-99");
                $(this).addClass("cpf-mask");
            }
        } else {
            if (!$(this).hasClass("cnpj-mask")) {
                $(this).mask("99.999.999/9999-99");
                $(this).addClass("cnpj-mask");
            }
        }
    });

    // Para telefone
    $("#phone").on("input", function () {  
        var elem = this;
        $(this).mask("(99)99999-9999");
        elem.selectionStart = elem.selectionEnd = elem.value.length;
    });
});