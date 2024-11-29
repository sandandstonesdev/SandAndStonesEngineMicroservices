import { Link } from "react-router-dom";
import { axiosInstance } from "../hooks/useAxios";
import { useEffect, useState } from "react";
import { isAxiosError } from "axios";
import { UserProfile } from "../types/UserProfile";
import UserProfileInfo from "../components/UserProfileInfo";

function Profile() {
    const [error, setError] = useState<string>("");
    const [isLoading, setIsLoading] = useState(false);
    const [userProfile, setUserProfile] = useState<UserProfile>();

    useEffect(() => {
        const getUserInfo = async () => {
            try {
                setIsLoading(true);

                const response = await axiosInstance.get(
                    `${import.meta.env.VITE_APP_BASE_URL}/gateway-api/userProfile/profile`,
                );

                const userProfileInfo = response.data as UserProfile;
                setUserProfile(userProfileInfo);

                console.log(response.data);
            }
            catch (err: unknown) {
                if (isAxiosError(err)) {
                    if (err.response) {
                        setError(`UserProfile send but fetched failed. Error ${err.status}`);
                    }
                    else {
                        setError(`UserProfile send but fetched failed.`);
                        console.error(err);
                    }
                }
            }
            finally {
                setIsLoading(false);
            }
        }

        getUserInfo();
    }, []);

    return (
        <>
            <h3>Hello! It's your profile info page.</h3>
            {isLoading && <div>Loading...</div>}
            {!isLoading && (<UserProfileInfo userProfile={userProfile} />)}
            <br/>
            <Link to="/logout">
                <span>Logout</span>
            </Link>
            {error && <p>{error}</p>}
        </>
      );
}

export default Profile;