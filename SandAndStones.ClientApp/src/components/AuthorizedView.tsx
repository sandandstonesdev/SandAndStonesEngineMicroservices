import React, { useState, useEffect } from 'react';
import { Navigate } from 'react-router-dom';
import { UserContext } from './UserContext';
import { User } from '../types/User';

function AuthorizedView(props: { children: React.ReactNode }) {

    const [authorized, setAuthorized] = useState<boolean>(false);
    const [loading, setLoading] = useState<boolean>(true); // add a loading state
    const emptyuser: User = { email: "", password: "" };

    const [user, setUser] = useState(emptyuser);

    useEffect(() => {
        // Get the cookie value
        let retryCount = 0; // initialize the retry count
        const maxRetries = 10; // set the maximum number of retries
        const delay: number = 1000; // set the delay in milliseconds

        // define a delay function that returns a promise
        function wait(delay: number) {
            return new Promise((resolve) => setTimeout(resolve, delay));
        }

        // define a fetch function that retries until status 200 or 401
        async function fetchWithRetry(url: string, options: RequestInit) {
            try {
                // make the fetch request
                const response = await fetch(url, options);

                // check the status code
                if (response.status == 200) {
                    console.log("Authorized");
                    const user: User = await response.json();
                    setUser({ email: user.email, password: user.password });
                    setAuthorized(true);
                    return response; // return the response
                } else if (response.status == 401) {
                    console.log("Unauthorized");
                    return response; // return the response
                } else {
                    // throw an error to trigger the catch block
                    throw new Error("" + response.status);
                }
            } catch (error) {
                // increment the retry count
                retryCount++;
                // check if the retry limit is reached
                if (retryCount > maxRetries) {
                    // stop retrying and rethrow the error
                    throw error;
                } else {
                    // wait for some time and retry
                    await wait(delay);
                    return fetchWithRetry(url, options);
                }
            }
        }

        // call the fetch function with retry logic
        fetchWithRetry("/pingauth", {
            method: "GET",
        })
            .catch((error) => {
                // handle the final error
                console.log(error.message);
            })
            .finally(() => {
                setLoading(false);  // set loading to false when the fetch is done
            });
    }, []);


    if (loading) {
        return (
            <>
                <p>Loading...</p>
            </>
        );
    }
    else {
        if (authorized && !loading) {
            return (
                <>
                    <UserContext.Provider value={user}>
                        {props.children}
                    </UserContext.Provider>
                </>
            );
        } else {
            return (
                <>
                    <Navigate to="/login"></Navigate>
                </>
            )
        }
    }

}

export function AuthorizedUser(props: { value: string }) {
    const user = React.useContext(UserContext) as User;

    if (props.value == "email")
        return <>{user.email}</>;
    else
        return <></>
}

export default AuthorizedView;