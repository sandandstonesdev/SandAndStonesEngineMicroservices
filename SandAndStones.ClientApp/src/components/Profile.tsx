import { Link } from "react-router-dom";
import { User } from "../types/User";
import Navbar from "./Navbar";
import { useEffect, useState } from "react";
import { useUser } from "./AuthProvider";

function Profile() {
    const { getUser } = useUser();
    const [email, setEmail] = useState("");

    useEffect(() => {
        const user: User | null = getUser();
        if (user !== null) {
            setEmail(user.email);
        }
    }, [email, getUser]);

    return (
        <>
            <Navbar />
            <p>Hello! It's your profile info page.</p><br />
            <Link to="/logout">
                <span>Logout {email}</span>
              </Link>
          </>
      );
}

export default Profile;