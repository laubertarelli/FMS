<script setup>
const getToken = () => localStorage.getItem("token");
function getTokenPayload() {
    var base64Url = getToken().split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
};

const isAdmin = getTokenPayload().role === "admin";
</script>

<template>
    <div>
        <nav class="div-nav">
            <ul class="nav-links">
                <li>
                    <RouterLink to="/files/1">Files</RouterLink>
                </li>
                <li>
                    <RouterLink to="/procedures/1">Procedures</RouterLink>
                </li>
                <li v-if="isAdmin">
                    <RouterLink to="/users/1">Users</RouterLink>
                </li>
            </ul>
            <RouterLink :to="{ name: 'Account' }"><button class="btn-header">Account</button></RouterLink>
        </nav>
    </div>
</template>

<style>
.div-nav {
    display: flex;
    align-items: baseline;
}

.nav-links {
    list-style: none;
}

.nav-links li {
    display: inline-block;
    padding: 0px 18px;
    font-weight: 500;
    font-size: 16px;
    color: #edf0f1;
    text-decoration: none;
}

.nav-links li a {
    transition: all 0.3s ease 0;
    font-weight: 500;
    font-size: 16px;
    color: #edf0f1;
    text-decoration: none;
}

.btn-header {
    margin-left: 20px;
    padding: 9px 25px;
    background-color: #1b6ec2;
    border: none;
    border-radius: 50px;
    cursor: pointer;
    font-weight: 500;
    font-size: 16px;
    color: #edf0f1;
    text-decoration: none;
}

.btn-header:hover {
    background-color: #0b5ed7;
}
</style>