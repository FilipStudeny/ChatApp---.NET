import { AccountBox, AppRegistration, Login, Logout, Message, Settings } from "@mui/icons-material";
import {
	Avatar,
	Container,
	Divider,
	Link,
	List,
	ListItem,
	ListItemButton,
	ListItemIcon,
	ListItemText,
	Stack,
} from "@mui/material";
import React from "react";
import { Link as RouterLink } from "react-router-dom";

import SidePanel from "./SidePanel";

const UserPanel: React.FunctionComponent = () => (
	<SidePanel>
		<Container sx={{ p: 1 }}>
			<Avatar
				alt='User image'
				src='/user_icon.png'
				sx={{ width: 100, height: 100, m: "auto", bgcolor: "white" }}
				variant='rounded'
			/>
		</Container>
		<Divider sx={{ borderBottomWidth: 2, bgcolor: "white" }} />
		<Stack justifyContent='space-between' sx={{ height: 1 }}>
			<List>
				<ListItem disablePadding>
					<Link component={RouterLink} to='/' color='inherit' underline='none' sx={{ width: 1 }}>
						<ListItemButton>
							<ListItemIcon>
								<AccountBox />
							</ListItemIcon>
							<ListItemText primary='Profile' />
						</ListItemButton>
					</Link>
				</ListItem>
				<ListItem disablePadding>
					<Link component={RouterLink} to='/messages' color='inherit' underline='none' sx={{ width: 1 }}>
						<ListItemButton>
							<ListItemIcon>
								<Message />
							</ListItemIcon>
							<ListItemText primary='Messages' />
						</ListItemButton>
					</Link>
				</ListItem>
				<ListItem disablePadding>
					<Link component={RouterLink} to='/settings' color='inherit' underline='none' sx={{ width: 1 }}>
						<ListItemButton>
							<ListItemIcon>
								<Settings />
							</ListItemIcon>
							<ListItemText primary='Settings' />
						</ListItemButton>
					</Link>
				</ListItem>
			</List>
			<List sx={{ borderTop: 2 }}>
				<ListItem disablePadding>
					<Link component={RouterLink} to='/login' color='inherit' underline='none' sx={{ width: 1 }}>
						<ListItemButton>
							<ListItemIcon>
								<Login />
							</ListItemIcon>
							<ListItemText primary='Sign in' />
						</ListItemButton>
					</Link>
				</ListItem>
				<ListItem disablePadding>
					<Link component={RouterLink} to='/loggout' color='inherit' underline='none' sx={{ width: 1 }}>
						<ListItemButton>
							<ListItemIcon>
								<Logout />
							</ListItemIcon>
							<ListItemText primary='Sign out' />
						</ListItemButton>
					</Link>
				</ListItem>
				<ListItem disablePadding>
					<Link component={RouterLink} to='/register' color='inherit' underline='none' sx={{ width: 1 }}>
						<ListItemButton>
							<ListItemIcon>
								<AppRegistration />
							</ListItemIcon>
							<ListItemText primary='Sign up' />
						</ListItemButton>
					</Link>
				</ListItem>
			</List>
		</Stack>
	</SidePanel>
);

export default UserPanel;

/*                <Avatar alt="User image" src='/user_icon.png' sx={{ width: 100, height: 100 }} variant="rounded" />
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
                */
