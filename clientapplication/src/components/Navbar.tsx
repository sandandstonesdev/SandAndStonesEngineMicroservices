import { NavLink } from "react-router-dom"

function Navbar() {
    return (
        <nav className="nav">
            <NavLink to="/home">Home</NavLink>
            <NavLink to="/register">Register</NavLink>
            <NavLink to="/login">Login</NavLink>
            <NavLink to="/about">About</NavLink>
        </nav>
  );
}

export default Navbar;