import pandas as pd
import os


def read_csv_to_dataframe(file_path):
    try:
        # Read the CSV file into a DataFrame
        df = pd.read_csv(file_path, sep=';')
        return df
    except FileNotFoundError:
        print(f"Error: File not found at path {file_path}")
        return None
    except Exception as e:
        print(f"An error occurred: {e}")
        return None


def get_folder(x):
    # Define the base directory where your participant folders are located
    base_directory = "Data"

    # Loop through participant folders
    for participant_number in range(1, x):
        participant_folder = os.path.join(base_directory, f"Participant {participant_number}")

        # Loop through Battery and Interval folders
        for subfolder in ["Battery", "Interval"]:
            subfolder_path = os.path.join(participant_folder, subfolder)

            if subfolder == "Battery":
                type_fold = 0
            elif subfolder == "Interval":
                type_fold = 1

            # Find all files in the subfolder
            all_files = os.listdir(subfolder_path)
            print(all_files)
            # Filter files that start with the common file name prefix
            relevant_files = [file for file in all_files if file.endswith("Game.csv") or file.endswith("Sample.csv")]
            print(relevant_files)
            # Loop through relevant files
            for file in relevant_files:
                file_path = os.path.join(subfolder_path, file)
                print(file_path)
                # Check if the file ends with "game" or "sample"
                if file.endswith("Game.csv"):
                    data_frame = read_csv_to_dataframe(file_path)
                    print('Game file loaded')
                elif file.endswith("Sample.csv"):
                    # Read CSV file using pandas
                    data_frame2 = read_csv_to_dataframe(file_path)
                    print('Sample file loaded')
                else:
                    print(f"Unrecognized file type: {file_path}")
            results = count_events_up_to_specific_event(data_frame, type_fold, data_frame2)
            results.to_csv('Data/Outputs_all/Output_' + str(participant_number) + '_' + str(type_fold))


def grab_buffer(df, frame):
    # Assuming df is the DataFrame and 'Framecount' is the column name
    closest_index = (df['Framecount'] - frame < 0).abs().idxmin()

    # Check if any index was found
    if closest_index:
        index_value = closest_index
        start_index = max(0, index_value - 8)  # Ensure not to go below index 0
        end_index = index_value - 1   # Include the index of the value
        confidences = df.loc[start_index:end_index, 'BCIConfidence'].values
        return confidences
    else:
        print(f"No matching index found for frame {frame}")
        return None


def count_events_up_to_specific_event(data_framer, typer, df2):
    # Ensure that the specified columns exist in the DataFrame
    # Assuming output is initially a DataFrame or you create it as an empty DataFrame
    output = pd.DataFrame(columns=['Index', 'Frame', 'Success', 'Goblin Death', 'Player Hurt',
                                   'Player Attacking', 'Player moving', 'Threshbuffer', 'type'])

    # Counters for each event type
    goblin_death_count = 0
    player_hurt_count = 0
    player_attacking_count = 0
    player_moving_count = 0
    row_num = 0

    # Iterate over the 'Event' column
    for index, column in data_framer.iterrows():
        event = column['Event']
        if event == 'Goblin Death':
            goblin_death_count += 1
        elif event == 'Player Hurt':
            player_hurt_count += 1
        elif event == 'Player Attacking':
            player_attacking_count += 1
        elif event == 'Player Moving':
            player_moving_count += 1
        elif event in ['BciSuccess', 'BciFail']:
            # Add a new row to the DataFrame for BCISuccess or BCIFail
            success_value = 1 if event == 'BciSuccess' else 0
            frames = data_framer['Framecount'][index]
            threshbuffer = grab_buffer(df2, frames)
            new_row = {'Index': index, 'Frame': frames, 'Success': success_value, 'Goblin Death': goblin_death_count,
                       'Player Hurt': player_hurt_count, 'Player Attacking': player_attacking_count,
                       'Player moving': player_moving_count, 'Threshbuffer': threshbuffer, 'type': typer}
            # Convert the new row to a DataFrame
            new_row_df = pd.DataFrame([new_row], columns=output.columns)

            # Concatenate the new row DataFrame to the output DataFrame
            output = pd.concat([output, new_row_df], ignore_index=True)
            print(f"Processed row {index}, output shape: {output.shape}")

        row_num += 1

    return output

get_folder(15)
