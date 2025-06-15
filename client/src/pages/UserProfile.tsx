import { useEffect, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import { profileDataValidationSchema } from "../validations/userProfile";
import { useNavigate, useParams } from "react-router";
import { GetUserProfileInfo, UpdateUserProfile } from "../services/userService";
import { Button, Stack, TextField, Typography } from "@mui/material";
import type { UserType, UserProfileType } from "../types/user.type";

const UserProfile = () => {
  const navigate = useNavigate();
  const { orm, userId } = useParams();
  const [user, setUser] = useState<UserType>();
  const {
    control,
    handleSubmit,
    formState: { errors, isDirty },
    reset,
  } = useForm({
    resolver: yupResolver(profileDataValidationSchema),
  });

  useEffect(() => {
    async function GetUserInfo() {
      if (userId) {
        const useDapper = orm === "dapper";
        const data = await GetUserProfileInfo(userId, useDapper);
        const newDate = {
          ...data,
          location: data.location ?? "",
          aboutMe: data.aboutMe ?? "",
        };
        setUser(newDate);
        reset(newDate);
      }
    }

    GetUserInfo();
  }, [userId, orm, reset]);

  const onSubmit = async (values: UserProfileType) => {
    if (userId && isDirty) {
      const useDapper = orm === "dapper";
      const data = await UpdateUserProfile(userId, values, useDapper);
      reset(data);
    }
  };

  const handleNavigateBack = () => {
    if (orm) {
      navigate(-1);
    }
  };

  return (
    <Stack gap={6}>
      <Typography variant="h6">Update User Profile</Typography>
      <Stack component="form" gap={3} onSubmit={handleSubmit(onSubmit)}>
        <Stack flexDirection="row" gap={2} flexWrap="wrap">
          <Controller
            control={control}
            name="displayName"
            defaultValue=""
            render={({ field }) => (
              <TextField
                {...field}
                sx={{ flexGrow: 1 }}
                label="Username"
                required
                type="text"
                error={!!errors.displayName}
                helperText={errors.displayName?.message}
              />
            )}
          />

          <Controller
            control={control}
            name="email"
            defaultValue=""
            render={({ field }) => (
              <TextField
                {...field}
                sx={{ flexGrow: 1 }}
                label="Email"
                type="text"
                error={!!errors.email}
                helperText={errors.email?.message}
              />
            )}
          />
        </Stack>

        <Stack flexDirection="row" gap={2} flexWrap="wrap">
          <Controller
            control={control}
            name="location"
            defaultValue=""
            render={({ field }) => (
              <TextField
                {...field}
                sx={{ flexGrow: 1 }}
                label="Location"
                type="text"
                error={!!errors.location}
                helperText={errors.location?.message}
              />
            )}
          />

          <Controller
            control={control}
            name="age"
            defaultValue={0}
            render={({ field }) => (
              <TextField
                {...field}
                label="Age"
                type="number"
                error={!!errors.age}
                helperText={errors.age?.message}
              />
            )}
          />
        </Stack>

        <Controller
          control={control}
          name="websiteUrl"
          defaultValue=""
          render={({ field }) => (
            <TextField
              {...field}
              label="Website URL"
              type="text"
              error={!!errors.websiteUrl}
              helperText={errors.websiteUrl?.message}
            />
          )}
        />

        <Controller
          control={control}
          name="aboutMe"
          defaultValue=""
          render={({ field }) => (
            <TextField
              {...field}
              sx={{ flexGrow: 1 }}
              label="About Me"
              multiline
              type="text"
              error={!!errors.aboutMe}
              helperText={errors.aboutMe?.message}
            />
          )}
        />

        <Stack flexDirection="row" gap={2} flexWrap="wrap">
          <TextField
            disabled
            sx={{ flexGrow: 1 }}
            value={user?.reputation ?? 0}
            label="Reputation"
            type="text"
          />

          <TextField
            disabled
            sx={{ flexGrow: 1 }}
            value={user?.upVotes ?? 0}
            label="Up Votes"
            type="text"
          />

          <TextField
            disabled
            sx={{ flexGrow: 1 }}
            value={user?.downVotes ?? 0}
            label="Down Votes"
            type="text"
          />
        </Stack>

        <Stack flexDirection="row" gap={2} flexWrap="wrap">
          <TextField
            disabled
            value={user?.views ?? 0}
            label="Views"
            type="text"
          />

          {user && (
            <TextField
              disabled
              sx={{ flexGrow: 1 }}
              value={new Date(user?.creationDate).toDateString() ?? ""}
              label="Creation Date"
              type="text"
            />
          )}
        </Stack>

        <Stack flexDirection="row" justifyContent="space-between">
          <Button onClick={handleNavigateBack}>Back</Button>
          <Button type="submit" variant="contained">
            Submit
          </Button>
        </Stack>
      </Stack>
    </Stack>
  );
};

export default UserProfile;
