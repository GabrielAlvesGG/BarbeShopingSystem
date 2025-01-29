$(document).ready(function () {
    // Para CPF e CNPJ
    var input = $('#cpf-cnpj');

    input.on('input', function () {
        var value = input.val().replace(/\D/g, ''); // Remove caracteres não numéricos
        var caret = this.selectionStart; // Salva a posição do cursor

        // Se ultrapassar 11 dígitos, aplica a máscara de CNPJ
        if (value.length > 11) {
            input.unmask().mask('00.000.000/0000-00');
        } else {
            input.unmask().mask('000.000.000-009'); // Mantém a máscara de CPF permitindo a transição
        }

        // Restaura a posição do cursor
        this.setSelectionRange(caret, caret);
    });

    // Para telefone
    $("#phone").mask("(00) 00000-0000");
});