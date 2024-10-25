import { RouterProvider, createBrowserRouter } from "react-router-dom";
import Login from "../pages/Login";
import About from "../pages/About";
import Register from "../pages/Register";
import Profile from "./Profile";
import Assets from "../pages/Assets";
import Textures from "../pages/Textures";
import Logout from "./Logout";
import { useAuth } from "../context/AuthProvider";
import ProtectedRoute from "./ProtectedRoute";
import Home from "../pages/Home";

interface AuthObjectToken {
    token : string | null
}

const Routes = () => {
    const { token } = useAuth() as AuthObjectToken;

    const publicRoutes = [
        {
            path: "/",
            element: <Login />,
            children:
                [
                    {
                        path: '/login',
                        element: <Login />,
                    },
                    {
                        path: '/register',
                        element: <Register />,
                    }
                ]
        }
    ];

    const authRoutes = [
        {
            path: "/",
            element: <ProtectedRoute><Home /></ProtectedRoute>,
            children: [
                {
                    path: '/about',
                    element: <About />,
                },
                {
                    path: '/profile',
                    element: <Profile />,
                },
                {
                    path: '/assets',
                    element: <Assets />,
                },
                {
                    path: '/textures',
                    element: <Textures />,
                },
                {
                    path: '/logout',
                    element: <Logout />,
                }
            ]
        },
    ];

    const router = createBrowserRouter([
        ...(token ? authRoutes : publicRoutes)
    ]);

    return <RouterProvider router={router} />;
};

export default Routes;