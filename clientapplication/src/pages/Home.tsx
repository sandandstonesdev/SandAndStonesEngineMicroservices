import { Outlet } from "react-router-dom";
import AuthProvider from "../components/AuthProvider.tsx";
import Navbar from "../components/Navbar.tsx";

function Home() {
    return (
        <div>
            <h1>SandAndStonesDev</h1>
            <div>
                <AuthProvider>
                    <Navbar />
                    <Outlet />
                </AuthProvider>
            </div>
        </div>
    );
}

export default Home;