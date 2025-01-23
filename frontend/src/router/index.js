import { createRouter, createWebHistory } from "vue-router";
import state from "@/state";
import HomeView from "@/views/HomeView.vue";
import FilesView from "@/views/FilesView.vue";
import ProceduresView from "@/views/ProceduresView.vue";
import UsersView from "@/views/UsersView.vue";
import AccountView from "@/views/AccountView.vue";
import Login from "@/components/LogIn.vue";


const routes = [
    {
        path: "/",
        name: "Login",
        component: Login
    },
    {
        path: "/home",
        name: "Home",
        component: HomeView
    },
    {
        path: "/files",
        name: "Files",
        component: FilesView
    },
    {
        path: "/procedures",
        name: "Procedures",
        component: ProceduresView
    },
    {
        path: "/users",
        name: "Users",
        component: UsersView
    },
    {
        path: "/account",
        name: "Account",
        component: AccountView
    }
]

const router = createRouter({
    routes,
    history: createWebHistory()
});

router.beforeEach((to) => {
    if (to.name !== "Login" && !state.token) {
        return { name: "Login" }
    }
});

export default router;