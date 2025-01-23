<script setup>

</script>

<template>
    <div>
        <nav class="div-nav">
            <ul class="nav-links">
                <li>
                    <RouterLink :to="{ name: 'Files' }">Files</RouterLink>
                </li>
                <li>
                    <RouterLink :to="{ name: 'Procedures' }">Procedures</RouterLink>
                </li>
                <li>
                    <RouterLink :to="{ name: 'Users' }">Users</RouterLink>
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