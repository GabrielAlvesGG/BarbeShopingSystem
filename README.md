# Fluxo de Login

## 1. Objetivo  
- Autenticar usuários nos perfis **Administrador**, **Barbeiro** ou **Cliente**.  
- Exibir/ocultar funcionalidades conforme o perfil.  
- Disponibilizar link para **Cadastrar novo usuário** e **Esqueci minha senha**.

## 2. Pré-requisitos  
- Conta pré-cadastrada (pelo Administrador) **ou** “Entrar com Google”.  
- No **primeiro acesso via Google**, após selecionar o e-mail, definir uma **senha local**.

## 3. Visão do usuário (UI)  
- **Inputs**: Email, Senha  
- **Botões/links**: Entrar, Entrar com Google, Cadastrar novo usuário, Esqueci minha senha  
- **Validações**:  
  - Campo em branco/inválido → mensagem em vermelho abaixo do input.  
  - Credenciais incorretas → modal de erro (“O seu login está incorreto!”).

## 4. Rotas do `LoginController`

1. **GET** `/Login`  
   Exibe o formulário de login.

2. **POST** `/LoginUser`  
   - **Body**
     ```json
     {
       "Email": "usuario@exemplo.com",
       "Password": "suaSenha123"
     }
     ```  
   - **Respostas**
     - `200 OK` → `true`  
     - `401 Unauthorized` → `false`  
     - `400 Bad Request` → campo faltando

3. **POST** `/Logout`  
   - **Body**: _nenhum_  
   - **Retorno**: `302 Redirect` para `/Login`

4. **GET** `/Login/SetPassword?email={email}`  
   Exibe o formulário para definir senha local (primeiro acesso via Google).

5. **POST** `/Login/SetPasswordConfirm`  
   - **Headers**
     ```http
     Content-Type: application/json
     ```
   - **Body** (JSON string literal)
     ```json
     "MinhaNovaSenha@123"
     ```  
   - **Respostas**
     - `200 OK` → senha salva com sucesso  
     - `400 Bad Request` → senha inválida ou faltando

## 5. Fluxo condicional

- **Login normal**  
  - `POST /LoginUser` com email+senha.  
  - **Sucesso** → `/Home` ou volta a `/Scheduling`.  
  - **Falha** → modal de erro.

- **Login com Google**  
  1. Clica “Entrar com Google” → **GET /signin-google**.  
  2. Seleciona e-mail:  
     - Já cadastrado → login normal.  
     - Primeiro acesso → **GET /Login/SetPassword?email={email}**.

- **Esqueci minha senha** … *(mesmo fluxo que você já tem em RecoveryPasswordController)*

## 6. Cenários de sucesso e falha

- **Sucesso** → redireciona corretamente.  
- **Falha de credenciais** → modal “O seu login está incorreto!”.  
- **Validação de formulário** → mensagens em vermelho.  
- **Erro no servidor** → mensagem genérica “Tente novamente mais tarde”.

## 7. Exemplos

```bash
curl -X POST https://seusite.com/LoginUser \
  -H "Content-Type: application/json" \
  -d '{"Email":"joao@exemplo.com","Password":"abc123"}'
