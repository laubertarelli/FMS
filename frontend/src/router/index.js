import { createRouter, createWebHistory } from "vue-router";
//import state from "@/state";
import HomeView from "@/views/HomeView.vue";
import FilesView from "@/components/files/FilesView.vue";
import ProceduresView from "@/components/procedures/ProceduresView.vue";
import UsersView from "@/components/users/UsersView.vue";
import AccountView from "@/views/AccountView.vue";
import Login from "@/components/LogIn.vue";
import FileDetails from "@/components/files/FileDetails.vue";
import ProcedureDetails from "@/components/procedures/ProcedureDetails.vue";
import UserDetails from "@/components/users/UserDetails.vue";
import AddFile from "@/components/files/AddFile.vue";
import UpdateFile from "@/components/files/UpdateFile.vue";
import UpdateProcedure from "@/components/procedures/UpdateProcedure.vue";
import AddProcedure from "@/components/procedures/AddProcedure.vue";


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
        path: "/files/:page",
        name: "Files",
        props: true,
        component: FilesView
    },
    {
        path: "/files/details/:id",
        name: "File Details",
        props: true,
        component: FileDetails
    },
    {
        path: "/files/add",
        name: "Add File",
        component: AddFile
    },
    {
        path: "/files/update/:id",
        name: "Update File",
        props: true,
        component: UpdateFile
    },
    {
        path: "/procedures/:page",
        name: "Procedures",
        props: true,
        component: ProceduresView
    },
    {
        path: "/procedures/details/:id",
        name: "Procedure Details",
        props: true,
        component: ProcedureDetails
    },
    {
        path: "/procedures/update/:id",
        name: "Update Procedure",
        component: UpdateProcedure
    },
    {
        path: "/procedures/add",
        name: "Add Procedure",
        component: AddProcedure
    },
    {
        path: "/users/:page",
        name: "Users",
        props: true,
        component: UsersView
    },
    {
        path: "/users/details/:id",
        name: "User Details",
        props: true,
        component: UserDetails
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

/*router.beforeEach((to) => {
    if (to.name !== "Login" && !state.token) {
        return { name: "Login" }
    }
});*/

router.afterEach((to) => {
    document.title = "FMS | " + to.name;
  })

export default router;