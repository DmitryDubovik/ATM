const accountsUrl = 'http://localhost:5009/api/accounts';


window.addEventListener('load', function () {
    getAccounts();
});


async function getAccounts() {

    try {
        const response = await fetch(`${accountsUrl}`);
        const accounts = await response.json();
        const accountType = document.getElementById('accountType');

        if (response.ok) {


            accounts.forEach(item => {
                const option = document.createElement('option');
                option.value = item.id;
                option.textContent = item.accountType;
                accountType.appendChild(option);
            });

            await getAccountInfo()
        } else {
            alert(`Error: ${account.message}`);
        }
    } catch (error) {
        alert(`Error: ${error.message}`);
    }
}


function accountTypeChanged() {
    getAccountInfo();
}

function getAccountId() {
    const accountType = document.getElementById('accountType');

    const selectedIndex = accountType.selectedIndex;
    const selectedOption = accountType.options[selectedIndex];

    const accountId = selectedOption.value;
    return accountId;
}


async function getAccountInfo() {

    const accountId = getAccountId();

    try {
        const response = await fetch(`${accountsUrl}/${accountId}`);
        const account = await response.json();

        if (response.ok) {
            let accountInfoElement = document.getElementById("accountInfo")
            accountInfoElement.innerHTML = `
                <p><strong>Account Type:</strong> ${account.accountType}</p>
                <p><strong>Account Number:</strong> ${account.accountNumber}</p>
                <p><strong>Balance:</strong> $${account.balance.toFixed(2)}</p>
            `;

            var x = document.getElementById("transactionHistory");
            if (x.style.display === "block") {
                loadTransactions();
            }

        } else {
            alert(`Error: ${account.message}`);
        }
    } catch (error) {
        alert(`Error: ${error.message}`);
    }
}

async function toggleTransactions() {
    const button = document.getElementById("toggleTransactionsButton");
    var x = document.getElementById("transactionHistory");
    if (x.style.display === "none") {
        x.style.display = "block";
        await loadTransactions();
        button.innerHTML = "<b>Hide Transaction History</b>";

    } else {
        x.style.display = "none";
        button.innerHTML = "<b>Show Transaction History</b>";
    }
}


async function loadTransactions() {
    const accountId = getAccountId();

    try {
        const response = await fetch(`${accountsUrl}/${accountId}/transactions`);
        const transactions = await response.json();

        if (response.ok) {

            var transactionHistoryElement = document.getElementById("transactionHistory");
            transactionHistoryElement.innerHTML = `
                <h3>Transaction History:</h3>
                <ul>
                    ${transactions.map(tx => formatTranaction(tx)).join('')}
                </ul>
            `;
        } else {
            alert(`Error: ${account.message}`);
        }
    } catch (error) {
        alert(`Error: ${error.message}`);
    }

}


function formatTranaction(tx) {

    const formatter = new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD',
      });

      const formattedAmount = formatter.format(tx.amount);
      const formattedBalance = formatter.format(tx.amount);

    const dateString = tx.date;

    const date = new Date(dateString);

    const formattedDate = date.toLocaleDateString('en-US', {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit',
        second: '2-digit',
    });

    return `<li>${formattedDate} ${tx.type} Amount: ${formattedAmount} Account Balance: ${formattedBalance}</li>`
}

async function deposit() {
    const amount = parseFloat(document.getElementById('amount').value);
    const accountId = getAccountId();

    if (amount <= 0) {
        alert("Please enter a valid amount greater than zero.");
        return;
    }

    try {
        const response = await fetch(`${accountsUrl}/${accountId}/deposit`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(amount)
        });

        const result = await response.text();

        if (response.ok) {
            getAccountInfo();



        } else {
            alert(`Error: ${result.message}`);
        }
    } catch (error) {
        alert(`Error: ${error.message}`);
    }
}

async function withdraw() {
    const amount = parseFloat(document.getElementById('amount').value);
    const accountId = getAccountId();

    if (amount <= 0) {
        alert("Please enter a valid amount greater than zero.");
        return;
    }

    try {
        const response = await fetch(`${accountsUrl}/${accountId}/withdraw`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(amount)
        });

        if (response.ok) {
            getAccountInfo();  // Refresh account info
        } else {
            alert(`Error: ${result.message}`);
        }
    } catch (error) {
        alert(`Error: ${error.message}`);
    }
}

async function transfer() {
    const amount = parseFloat(document.getElementById('amount').value);
    const accountId = getAccountId();

    if (amount <= 0) {
        alert("Please enter a valid amount greater than zero.");
        return;
    }

    try {
        const response = await fetch(`${accountsUrl}/${accountId}/transfer`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(amount)
        });

        const result = await response.text();

        if (response.ok) {
            getAccountInfo();  // Refresh account info
        } else {
            alert(`Error: ${result.message}`);
        }
    } catch (error) {
        alert(`Error: ${error.message}`);
    }
}



