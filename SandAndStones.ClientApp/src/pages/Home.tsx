import { Outlet } from "react-router-dom";
import Navbar from "../components/Navbar.tsx";

function Home() {
    return (
        <div>
                <Navbar />
                <Outlet />
        </div>
    );
}

export default Home;