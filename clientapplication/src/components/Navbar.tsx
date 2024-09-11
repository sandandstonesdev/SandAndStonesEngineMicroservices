import { NavLink } from "react-router-dom"

function Navbar() {
    return (
        <nav className="nav">
            <NavLink to="/about">About</NavLink>
            <NavLink to="/profile">Profile</NavLink>
        </nav>
  );
}

export default Navbar;