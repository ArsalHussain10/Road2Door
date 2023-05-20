    let itemId;


    $(document).ready(function () {
        // Add click event listener to edit buttons
        $('.edit-btn').click(function () {
            // Get item ID from data attribute
            itemId = $(this).data('item-id');

            console.log(itemId);

            // Show modal
            $('#edit-modal').css('display', 'block');
        });

    // Add click event listener to close button in modal
    $('.close').click(function () {
        $('#edit-modal').css('display', 'none');
        });

    // Add click event listener to submit button in modal
    $('#edit-submit').click(function () {
            const description=document.getElementById("edit-description");
    const price=document.getElementById("edit-price");
    const quantity=document.getElementById("edit-quantity");
    const itemId1 = document.querySelector('#itemId');
    itemId1.value=itemId;
    console.log(itemId1);



    window.location.href = "/Rider/EditItem?itemId=" + itemId1 + "&description=" +description + "&price=" + price + "&quantity=" + quantity;



        });
    });


    const createBtn = document.getElementById("create-btn");
    const modal = document.getElementById("modal");
    const closeBtn = document.getElementsByClassName("close")[0];

    createBtn.onclick = function () {
        modal.style.display = "block";
    };

    closeBtn.onclick = function () {
        modal.style.display = "none";
    };

    window.onclick = function (event) {
        if (event.target == modal) {
        modal.style.display = "none";
        }
    };


    function deleteItem(itemId) {
        if (confirm("Are you sure you want to delete this item?")) {
        window.location.href = "/Rider/DeleteItem?itemId=" + itemId;
        }
    }




