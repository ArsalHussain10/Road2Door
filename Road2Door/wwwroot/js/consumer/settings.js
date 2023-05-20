

    function save() {
        console.log("Inside save function");
    const id = document.getElementById("consumer-id").value;
    const name = document.getElementById("name").value;
    const email = document.getElementById("email").value;
    const password = document.getElementById("password").value;
    const contact = document.getElementById("contact").value;
    console.log(id);
    console.log(name);
    console.log(email);
    console.log(password);
    console.log(contact);
    window.location.href = "/Consumer/EditSettings?id=" + id + "&name=" + name + "&email=" + email + "&password=" + password + "&contact=" + contact;

    }


