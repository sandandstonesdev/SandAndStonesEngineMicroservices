import { NavLink } from "react-router-dom"
import './InitialNavbar.css';

function InitialNavbar() {
    return (
        <nav className="initialnav">
            <NavLink to="/login">Login</NavLink>
            <NavLink to="/register">Register</NavLink>
        </nav>
    );
}

export default InitialNavbar;