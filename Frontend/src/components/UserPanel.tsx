import { Avatar, Divider, Link, List, ListItemButton, ListItemText, Paper } from '@mui/material';
import React from 'react'
import { Link as RouterLink } from 'react-router-dom'


const UserPanel: React.FunctionComponent = () => {
    return (
        <>
            <Paper elevation={5} sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', minWidth: 200 }} >
                <Avatar alt="User image" src='/user_icon.png' sx={{ width: 100, height: 100 }} variant="rounded" />
                <Divider variant="middle" />
                <List sx={{ width: 1 }}>
                    <Link component={RouterLink} to={'/profile'} color='inherit' underline="none">
                        <ListItemButton>
                            <ListItemText primary="Profile" />
                        </ListItemButton>
                    </Link>
                    <Link component={RouterLink} to={'/messages'} color='inherit' underline="none">
                        <ListItemButton>
                            <ListItemText primary="Messages" />
                        </ListItemButton>
                    </Link>
                    <Link component={RouterLink} to={'/settings'} color='inherit' underline="none">
                        <ListItemButton>
                            <ListItemText primary="Settings" />
                        </ListItemButton>
                    </Link>
                </List>
            </Paper>
        </>
    )
}

export default UserPanel